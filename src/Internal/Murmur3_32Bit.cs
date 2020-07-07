using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastHash.Internal
{
    internal sealed class Murmur3_32Bit : IHashFunction
    {
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;
        private uint _hash;
        private uint _totalBytesProcessed;

        public int BlockSizeBytes => 4;

        public void AddCompleteBlock(ReadOnlySpan<byte> block)
        {
            var k1 = MemoryMarshal.Read<uint>(block);
            k1 *= C1;
            k1 = k1.RotateLeft(15);
            k1 *= C2;

            var h1 = _hash;
            h1 ^= k1;
            h1 = h1.RotateLeft(13);
            h1 *= 5;
            h1 += 0xe6546b64;

            _hash = h1;
            _totalBytesProcessed += 4;
        }

        public void AddRemainder(ReadOnlySpan<byte> remainder)
        {
            uint k1 = 0;
            
            switch (remainder.Length & 3)
            {
                case 3: k1 ^= (uint)remainder[2] << 16; goto case 2;
                case 2: k1 ^= (uint)remainder[1] << 8; goto case 1;
                case 1: k1 ^= remainder[0];
                    k1 *= C1; k1 = k1.RotateLeft(15); k1 *= C2; _hash ^= k1;
                    break;
            }

            _totalBytesProcessed += (uint)remainder.Length;
        }

        public byte[] GetFinalHashValue()
        {
            var h1 = _hash;
            
            h1 ^= _totalBytesProcessed;

            h1 = FMix32(h1);
            
            var bytes = BitConverter.GetBytes(h1);

            Reset();

            return bytes;
        }

        private void Reset()
        {
            _hash = 0;
            _totalBytesProcessed = 0;
            Murmur3.Return(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FMix32(uint k)
        {
            k ^= k >> 16;
            k *= 0x85ebca6b;
            k ^= k >> 13;
            k *= 0xc2b2ae35;
            k ^= k >> 16;
            return k;
        }
    }
}