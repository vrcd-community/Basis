using System;
using Basis.Network.Core;

public static partial class SerializableBasis
{
    public struct VoiceReceiversMessage
    {
        // Hard cap to avoid giant allocations if data is corrupted
        private const int MaxUsers = 1024;

        /// <summary>
        /// Backing storage. May be larger than Count.
        /// </summary>
        public ushort[] Users;

        /// <summary>
        /// Number of valid entries in Users to serialize.
        /// </summary>
        public int Count;

        public void Deserialize(NetDataReader reader)
        {
            int remainingBytes = reader.AvailableBytes;

            // No data at all – treat as "no users"
            if (remainingBytes <= 0)
            {
                Users = Array.Empty<ushort>();
                Count = 0;
                return;
            }

            // Need at least 2 bytes for the length
            if (remainingBytes < sizeof(ushort))
            {
                BNL.LogError($"VoiceReceiversMessage: not enough bytes for length. Remaining={remainingBytes}");

                SkipRemaining(reader);
                Users = Array.Empty<ushort>();
                Count = 0;
                return;
            }

            // Read the count
            ushort countU16 = reader.GetUShort();
            int count = countU16;

            if (count == 0)
            {
                Users = Array.Empty<ushort>();
                Count = 0;
                return;
            }

            // Basic sanity check: avoid insane counts
            if (count > MaxUsers)
            {
                BNL.LogError($"VoiceReceiversMessage: reported count={count} exceeds MaxUsers={MaxUsers}. Possible protocol mismatch or corrupted packet.");
                SkipRemaining(reader);
                Users = Array.Empty<ushort>();
                Count = 0;
                return;
            }

            int bytesNeeded = count * sizeof(ushort);

            if (reader.AvailableBytes < bytesNeeded)
            {
                BNL.LogError($"VoiceReceiversMessage: count={count} needs {bytesNeeded} bytes, but only {reader.AvailableBytes} available. Protocol mismatch?");
                SkipRemaining(reader);
                Users = Array.Empty<ushort>();
                Count = 0;
                return;
            }

            // Ensure buffer (reuse if possible)
            if (Users == null || Users.Length < count)
            {
                Users = new ushort[count];
            }

            for (int Index = 0; Index < count; Index++)
            {
                Users[Index] = reader.GetUShort();
            }

            Count = count;
        }

        public void Serialize(NetDataWriter writer)
        {
            int count = Count;

            if (Users == null || count <= 0)
            {
                writer.Put((ushort)0);
                return;
            }

            // Clamp to actual buffer size
            if (count > Users.Length)
            {
                count = Users.Length;
            }

            // Clamp to protocol max + ushort max
            if (count > MaxUsers)
            {
                BNL.LogError($"VoiceReceiversMessage: Count={count} exceeds MaxUsers={MaxUsers}. Truncating.");
                count = MaxUsers;
            }

            if (count > ushort.MaxValue)
            {
                BNL.LogError($"VoiceReceiversMessage: Count={count} exceeds ushort.MaxValue. Truncating.");
                count = ushort.MaxValue;
            }

            writer.Put((ushort)count);

            for (int Index = 0; Index < count; Index++)
            {
                writer.Put(Users[Index]);
            }
        }

        private static void SkipRemaining(NetDataReader reader)
        {
            if (reader.AvailableBytes > 0)
            {
                reader.SkipBytes(reader.AvailableBytes);
            }
        }
    }
}
