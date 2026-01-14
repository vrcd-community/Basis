using LiteNetLib.Layers;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Net;
using ZstdSharp;
using ZstdSharp.Unsafe;

public sealed class CompressionPacketLayer : PacketLayerBase
{
    // 1 byte flag + 4 bytes original length
    private const int HeaderSize = 5;

    private const byte FlagRaw = 0;
    private const byte FlagZstd = 2;

    // Tune these:
    private const int MinCompressBytes = 500;
    private const int ZstdLevel = 1; // 1 = fast. higher = smaller/slower.

    // Optional: only worth it for *big* packets; zstd threading has overhead.
    private const int MultiThreadMinBytes = 64 * 1024;

    // Zstd frame can expand slightly in worst-case; add a little slop.
    private const int ExtraSlop = 64;

    public CompressionPacketLayer() : base(64) { } // keep upstream headroom

    public override void ProcessOutBoundPacket(
        ref IPEndPoint endPoint,
        ref byte[] data,
        ref int offset,
        ref int length)
    {
        int originalLen = length;

        // RAW fast-path: prepend header via headroom (no payload shift).
        if (originalLen < MinCompressBytes)
        {
            WriteRawHeaderFast(ref data, ref offset, ref length);
            return;
        }

        // Compress into pooled expandable buffer (no per-packet MemoryStream allocations)
        // We start writing payload at HeaderSize so header lands at 0.
        using var outStream = new PooledMemoryStream(originalLen + ExtraSlop);
        outStream.Position = HeaderSize;

        // Zstd streaming compression. README shows CompressionStream(output, level). :contentReference[oaicite:3]{index=3}
        using (var zstream = new CompressionStream(outStream, ZstdLevel))
        {
            // If your ZstdSharp version has this (0.8.2+), it can help. :contentReference[oaicite:4]{index=4}
            TrySetPledgedSrcSize(zstream, originalLen);

            // Multi-thread only for large payloads. README shows SetParameter(nbWorkers). :contentReference[oaicite:5]{index=5}
            if (originalLen >= MultiThreadMinBytes)
            {
                // Usually: Environment.ProcessorCount, but tune for your server/client.
                zstream.SetParameter(ZSTD_cParameter.ZSTD_c_nbWorkers, Environment.ProcessorCount);
            }

            zstream.Write(data, offset, originalLen);
        }

        int compressedPayloadLen = (int)outStream.Length - HeaderSize;

        // If no win, send RAW (avoid useless CPU + expansion)
        if (compressedPayloadLen >= originalLen)
        {
            WriteRawHeaderFast(ref data, ref offset, ref length);
            return;
        }

        // Header at position 0 in pooled buffer
        WriteZstdHeader(outStream._buffer, originalLen);

        int totalLen = HeaderSize + compressedPayloadLen;

        // Copy compressed packet into output buffer with header headroom (keeps offset-based protocol fast)
        EnsureHeadroomAndCapacity(ref data, ref offset, totalLen);
        Buffer.BlockCopy(outStream._buffer, 0, data, offset, totalLen);
        length = totalLen;
    }

    public override void ProcessInboundPacket(ref IPEndPoint endPoint, ref byte[] data, ref int length)
    {
        if (length < HeaderSize) return;

        byte flag = data[0];

        if (flag == FlagRaw)
        {
            int payloadLen = length - HeaderSize;
            Buffer.BlockCopy(data, HeaderSize, data, 0, payloadLen);
            length = payloadLen;
            return;
        }

        if (flag != FlagZstd) return;

        int originalLen = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(1, 4));
        if (originalLen <= 0) return;

        // Decompress directly into a pooled buffer (no MemoryStream growth / CopyTo allocations)
        byte[] outBuf = ArrayPool<byte>.Shared.Rent(originalLen);
        try
        {
            using var input = new MemoryStream(data, HeaderSize, length - HeaderSize, writable: false);
            using var zstream = new DecompressionStream(input);

            int written = 0;
            while (written < originalLen)
            {
                int n = zstream.Read(outBuf, written, originalLen - written);
                if (n == 0) break;
                written += n;
            }

            if (written != originalLen) return; // corrupted / liar sender / truncated

            EnsureCapacity(ref data, originalLen);
            Buffer.BlockCopy(outBuf, 0, data, 0, originalLen);
            length = originalLen;
        }
        catch
        {
            // drop / stats
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(outBuf);
        }
    }

    private static void WriteZstdHeader(byte[] buffer, int originalLen)
    {
        buffer[0] = FlagZstd;
        BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(1, 4), originalLen);
    }

    // FAST: prepend header using offset headroom (no shifting payload every time).
    private static void WriteRawHeaderFast(ref byte[] data, ref int offset, ref int length)
    {
        EnsureHeadroomAndCapacity(ref data, ref offset, HeaderSize + length);

        data[offset + 0] = FlagRaw;
        BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(offset + 1, 4), length);
        length += HeaderSize;
    }

    // Ensures: (1) we have HeaderSize bytes of headroom by moving offset back,
    // (2) we have enough capacity in the underlying array for (offset + requiredLen).
    // Slow path (rare): if there isn't headroom, shift the packet once to create it.
    private static void EnsureHeadroomAndCapacity(ref byte[] data, ref int offset, int requiredLen)
    {
        if (offset >= HeaderSize)
        {
            offset -= HeaderSize;
            int end = offset + requiredLen;
            if (data.Length < end) Array.Resize(ref data, end);
            return;
        }

        int oldOffset = offset;
        int payloadLen = requiredLen - HeaderSize;

        int newOffset = HeaderSize;
        int end2 = newOffset + payloadLen + HeaderSize; // == requiredLen + HeaderSize
        if (data.Length < end2) Array.Resize(ref data, end2);

        if (payloadLen > 0)
            Buffer.BlockCopy(data, oldOffset, data, newOffset, payloadLen);

        offset = 0;
    }

    private static void EnsureCapacity(ref byte[] buffer, int required)
    {
        if (buffer.Length >= required) return;
        Array.Resize(ref buffer, required);
    }

    /// <summary>
    /// Best-effort call to CompressionStream.SetPledgedSrcSize(long) if present.
    /// Mentioned in ZstdSharp release notes. :contentReference[oaicite:6]{index=6}
    /// </summary>
    private static void TrySetPledgedSrcSize(CompressionStream stream, int size)
    {
        // Avoid hard dependency if you're pinned to an older ZstdSharp.
        // Reflection cost here is tiny vs compression; if you want *zero* reflection,
        // just call stream.SetPledgedSrcSize(size) directly and require 0.8.2+.
        var mi = stream.GetType().GetMethod("SetPledgedSrcSize", new[] { typeof(long) })
              ?? stream.GetType().GetMethod("SetPledgedSrcSize", new[] { typeof(ulong) })
              ?? stream.GetType().GetMethod("SetPledgedSrcSize", new[] { typeof(int) });

        if (mi == null) return;

        object arg = mi.GetParameters()[0].ParameterType == typeof(ulong)
            ? (object)(ulong)size
            : (object)(long)size;

        mi.Invoke(stream, new[] { arg });
    }

    /// <summary>
    /// Expandable stream backed by ArrayPool to avoid per-packet allocations.
    /// </summary>
    private sealed class PooledMemoryStream : Stream
    {
        public byte[] _buffer;
        private int _length;
        private int _pos;

        public PooledMemoryStream(int initialCapacity)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(Math.Max(512, initialCapacity));
            _length = 0;
            _pos = 0;
        }
        public override bool CanRead => false;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        public override long Length => _length;

        public override long Position
        {
            get => _pos;
            set
            {
                int v = (int)value;
                _pos = v;
                if (_pos > _length) _length = _pos;
            }
        }

        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin)
        {
            int newPos = origin switch
            {
                SeekOrigin.Begin => (int)offset,
                SeekOrigin.Current => _pos + (int)offset,
                SeekOrigin.End => _length + (int)offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };
            Position = newPos;
            return _pos;
        }

        public override void SetLength(long value)
        {
            int v = (int)value;
            EnsureCapacity(v);
            _length = v;
            if (_pos > _length) _pos = _length;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count <= 0) return;
            int end = _pos + count;
            EnsureCapacity(end);
            Buffer.BlockCopy(buffer, offset, _buffer, _pos, count);
            _pos = end;
            if (_pos > _length) _length = _pos;
        }

        public override void WriteByte(byte value)
        {
            int end = _pos + 1;
            EnsureCapacity(end);
            _buffer[_pos] = value;
            _pos = end;
            if (_pos > _length) _length = _pos;
        }

        private void EnsureCapacity(int required)
        {
            if (_buffer.Length >= required) return;

            int newSize = _buffer.Length;
            while (newSize < required) newSize <<= 1;

            byte[] newBuf = ArrayPool<byte>.Shared.Rent(newSize);
            if (_length > 0)
                Buffer.BlockCopy(_buffer, 0, newBuf, 0, _length);

            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = newBuf;
        }

        protected override void Dispose(bool disposing)
        {
            if (_buffer != null)
            {
                ArrayPool<byte>.Shared.Return(_buffer);
                _buffer = null!;
            }
            base.Dispose(disposing);
        }
    }
}
