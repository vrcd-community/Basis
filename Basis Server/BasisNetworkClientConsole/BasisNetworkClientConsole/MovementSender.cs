
using Basis.Network.Core;
using Basis.Network.Core.Compression;
using BasisNetworkClientConsole;
using static SerializableBasis;
using Basis.Scripts.Networking.Compression;
using System.Runtime.CompilerServices;
namespace Basis.Network
{
    public static class MovementSender
    {
        public static Quaternion Rotation;
        private const ushort UShortMin = ushort.MinValue;   // 0
        private const ushort UShortMax = ushort.MaxValue;   // 65535
        private const ushort UShortRangeDifference = UShortMax - UShortMin;
        public static Vector3[] PlayersCurrentPosition;
        public static PlayerData[] ActivePlayerData;

        public struct PlayerData
        {
            public NetDataWriter Writer;
            public LocalAvatarSyncMessage Message;
        }

        // Precompute compressed scale once; reused for all messages.
        private static readonly ushort CompressedScale = CompressScaleOnce(1);

        public static void Initialize(int clientCount)
        {
            Rotation = new Quaternion(0, 0, 0, 1);
            PlayersCurrentPosition = new Vector3[clientCount];
            ActivePlayerData = new PlayerData[clientCount];

            for (int Index = 0; Index < clientCount; Index++)
            {
                PlayersCurrentPosition[Index] = Randomizer.GetRandomOffset();
                ActivePlayerData[Index] = Generate();
            }
        }
        public static PlayerData Generate()
        {
            var pd = new PlayerData
            {
                Writer = new NetDataWriter(),
                Message = new LocalAvatarSyncMessage
                {
                    AdditionalAvatarDatas = null,
                    AdditionalAvatarDataSize = 0,
                    LinkedAvatarIndex = 0,
                    array = new byte[BasisBitPackingConstants.AvatarSyncSize],
                }
            };

            int offset = 0;
            var message = pd.Message;
            // Position (12 bytes)
            WritePosition(Randomizer.GetRandomOffset(), ref message.array, ref offset);//12

            // Rotation xyz (12 bytes) + compressed w (2 bytes)
            WriteQuaternionToBytes(Rotation, ref message.array, ref offset);//16

            // Scale (2 bytes) at the end
            int scaleOffset = BasisBitPackingConstants.AvatarSyncSize - 2;
            WriteUShort(CompressedScale, ref message.array, ref scaleOffset);//2
            pd.Message = message;
            return pd;
        }

        public static void ProcessSingle(NetPeer peer, int index)
        {
            if (peer == null) return;

            // Update position
            PlayersCurrentPosition[index] += Randomizer.GetRandomOffset();

            // Overwrite just the position region in the message buffer
            int offset = 0;
            var Message = ActivePlayerData[index].Message;
            WritePosition(PlayersCurrentPosition[index], ref Message.array, ref offset);
            var Writer = ActivePlayerData[index].Writer;
            // Reset writer before (re)serialization
            Writer.Reset();
            Message.Serialize(Writer);


            peer.Send(Writer, BasisNetworkCommons.PlayerAvatarChannel, DeliveryMethod.Sequenced);

            ActivePlayerData[index].Message = Message;
        }

        public static void WritePosition(Scripts.Networking.Compression.Vector3 position, ref byte[] buffer, ref int offset)
        {
            unsafe
            {
                fixed (byte* dst = &buffer[offset])
                {
                    float* f = (float*)dst;
                    f[0] = position.x;
                    f[1] = position.y;
                    f[2] = position.z;
                }
            }
            offset += 12;
        }

        public unsafe static void WriteQuaternionToBytes(Scripts.Networking.Compression.Quaternion q, ref byte[] bytes, ref int offset)
        {
            EnsureSpace(bytes, offset, 16); fixed (byte* ptr = &bytes[offset])
            {
                float* f = (float*)ptr; f[0] = q.value.x;
                f[1] = q.value.y;
                f[2] = q.value.z;
                f[3] = q.value.w;
            }
            offset += 16;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EnsureSpace(byte[] bytes, int offset, int size)
        {
            return (uint)offset <= (uint)bytes.Length && offset + size <= bytes.Length;
        }
        private static ushort CompressScaleOnce(float scale)
        {
            const float Min = 0.005f;
            const float Max = 150f;
            const float Range = Max - Min;
            float normalized = (scale - Min) / Range;
            ushort compressed = (ushort)(normalized * UShortRangeDifference);

            return compressed;
        }

        public static void WriteUShort(ushort value, ref byte[] bytes, ref int offset)
        {
            bytes[offset++] = (byte)value;
            bytes[offset++] = (byte)(value >> 8);
        }
    }
}
