using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

public partial class BasisTransmissionResults
{
    // ---------------------------
    // Burst Job
    // ---------------------------

    [BurstCompile]
    private struct BasisBuildRecipientsAndSignatureJob : IJob
    {
        [ReadOnly] public NativeArray<ushort> playerIds;
        [ReadOnly] public NativeArray<byte> inMicRange; // 0/1
        [ReadOnly] public int length;

        // Output: compacted recipients in first outCount entries
        public NativeArray<ushort> outUsers;

        // Output scalars (len 1)
        public NativeArray<int> outCount;
        public NativeArray<ulong> outHash;

        public void Execute()
        {
            int count = 0;
            ulong xorAcc = 0;
            ulong sumAcc = 0;

            for (int i = 0; i < length; i++)
            {
                if (inMicRange[i] == 0)
                    continue;

                ushort id = playerIds[i];
                outUsers[count++] = id;

                ulong h = HashUShort(id);
                xorAcc ^= h;
                sumAcc += (h | 1UL);
            }

            // order-independent-ish signature
            ulong combined = xorAcc ^ (sumAcc * 0x9E3779B97F4A7C15UL);
            combined ^= (ulong)count * 0xD6E8FEB86659FD93UL;

            outCount[0] = count;
            outHash[0] = combined;
        }

        private static ulong HashUShort(ushort v)
        {
            // 64-bit mixer
            ulong x = v;
            x ^= x >> 33;
            x *= 0xff51afd7ed558ccdUL;
            x ^= x >> 33;
            x *= 0xc4ceb9fe1a85ec53UL;
            x ^= x >> 33;
            return x;
        }
    }
}
