using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastHash.Internal
{
    internal sealed class Murmur3_128Bit : IHashFunction
    {
        private const ulong C1 = 0x87c37b91114253d5;
        private const ulong C2 = 0x4cf5ad432745937f;
        private ulong _hash1;
        private ulong _hash2;
        private uint _totalBytesProcessed;

        public int BlockSizeBytes => 16;

        public void AddCompleteBlock(ReadOnlySpan<byte> block)
        {
            var k1 = MemoryMarshal.Read<ulong>(block);
            var k2 = MemoryMarshal.Read<ulong>(block.Slice(8));

            var h1 = _hash1;
            var h2 = _hash2;
            
            k1 *= C1;
            k1 = k1.RotateLeft(31);
            k1 *= C2;
            h1 ^= k1;
            
            h1 = h1.RotateLeft(27);
            h1 += h2;
            h1 *= 5;
            h1 += 0x52dce729;

            k2 *= C2;
            k2 = k2.RotateLeft(33);
            k2 *= C1;
            h2 ^= k2;

            h2 = h2.RotateLeft(31);
            h2 += h1;
            h2 *= 5;
            h2 += 0x38495ab5;
            
            _hash1 = h1;
            _hash2 = h2;
            _totalBytesProcessed += 16;
        }

        public void AddRemainder(ReadOnlySpan<byte> remainder)
        {
            ulong k1 = 0;
            ulong k2 = 0;

            switch (remainder.Length & 15)
            {
                case 15: k2 ^= (ulong)remainder[14] << 48; goto case 14;
                case 14: k2 ^= (ulong)remainder[13] << 40; goto case 13;
                case 13: k2 ^= (ulong)remainder[12] << 32; goto case 12;
                case 12: k2 ^= (ulong)remainder[11] << 24; goto case 11;
                case 11: k2 ^= (ulong)remainder[10] << 16; goto case 10;
                case 10: k2 ^= (ulong)remainder[9] << 8; goto case 9;
                case 9: k2 ^= (ulong)remainder[8];
                    k2 *= C2; k2 = k2.RotateLeft(33); k2 *= C1; _hash2 ^= k2;
                    goto case 8;
                case 8: k1 ^= (ulong)remainder[7] << 56; goto case 7;
                case 7: k1 ^= (ulong)remainder[6] << 48; goto case 6;
                case 6: k1 ^= (ulong)remainder[5] << 40; goto case 5;
                case 5: k1 ^= (ulong)remainder[4] << 32; goto case 4;
                case 4: k1 ^= (ulong)remainder[3] << 24; goto case 3;
                case 3: k1 ^= (ulong)remainder[2] << 16; goto case 2;
                case 2: k1 ^= (ulong)remainder[1] << 8; goto case 1;
                case 1: k1 ^= (ulong)remainder[0];
                    k1 *= C1; k1 = k1.RotateLeft(31); k1 *= C2; _hash1 ^= k1;
                    break;
            }

            _totalBytesProcessed += (uint)remainder.Length;
        }

        public byte[] GetFinalHashValue()
        {
            var h1 = _hash1;
            var h2 = _hash2;

            h1 ^= _totalBytesProcessed;
            h2 ^= _totalBytesProcessed;

            h1 += h2;
            h2 += h1;

            h1 = FMix64(h1);
            h2 = FMix64(h2);

            h1 += h2;
            h2 += h1;

            var bytes = new byte[16];

            var span = MemoryMarshal.Cast<byte, ulong>(bytes.AsSpan());
            span[0] = h1;
            span[1] = h2;
            
            Reset();

            return bytes;
        }

        private void Reset()
        {
            _hash1 = 0;
            _hash2 = 0;
            _totalBytesProcessed = 0;
            Murmur3.Return(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong FMix64(ulong k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;
            return k;
        }
    }
}