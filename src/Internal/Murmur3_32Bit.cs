using System;
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

            var hash = _hash;
            hash ^= k1;
            hash = hash.RotateLeft(13);
            hash *= 5;
            hash += 0xe6546b64;

            _hash = hash;
            _totalBytesProcessed += 4;
        }

        public void AddRemainder(ReadOnlySpan<byte> remainder)
        {
            uint k1 = 0;
            
            switch (remainder.Length)
            {
                case 3: k1 ^= (uint)(remainder[2] << 16); goto case 2;
                case 2: k1 ^= (uint)(remainder[1] << 8); goto case 1;
                case 1: k1 ^= remainder[0];
                    k1 *= C1; k1 = k1.RotateLeft(15); k1 *= C2; _hash ^= k1;
                    break;
            }

            _totalBytesProcessed += (uint)remainder.Length;
        }

        public byte[] GetFinalHashValue()
        {
            var hash = _hash;
            
            hash ^= _totalBytesProcessed;

            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;
            
            var bytes = BitConverter.GetBytes(hash);

            Reset();

            return bytes;
        }

        private void Reset()
        {
            _hash = 0;
            _totalBytesProcessed = 0;
            Murmur3.Return(this);
        }
    }
}