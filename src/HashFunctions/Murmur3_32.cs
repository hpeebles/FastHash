using System;
using FastHash.Internal;

namespace FastHash.HashFunctions
{
    public sealed class Murmur3_32 : IHashFunction32
    {
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;
        
        private Murmur3_32() { }
        
        public static Murmur3_32 Instance { get; } = new Murmur3_32();

        public uint Seed => 0;

        public uint AddBlock(uint hash, uint nextBlock, long blockIndex)
        {
            var k1 = nextBlock;
            k1 *= C1;
            k1 = k1.RotateLeft(15);
            k1 *= C2;

            hash ^= k1;
            hash = hash.RotateLeft(13);
            hash *= 5;
            hash += 0xe6546b64;

            return hash;
        }

        public uint AddRemainder(uint hash, ReadOnlySpan<byte> remainder, long blockIndex)
        {
            uint k1 = 0;
            
            switch (remainder.Length)
            {
                case 3: k1 ^= (uint)(remainder[2] << 16); goto case 2;
                case 2: k1 ^= (uint)(remainder[1] << 8); goto case 1;
                case 1: k1 ^= remainder[0];
                    k1 *= C1; k1 = k1.RotateLeft(15); k1 *= C2; hash ^= k1;
                    break;
            }

            return hash;
        }

        public uint Finalize(uint hash, long lengthBytes)
        {
            hash ^= (uint)lengthBytes;
            
            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            return hash;
        }
    }
}