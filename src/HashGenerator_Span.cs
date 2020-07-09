using System;
using FastHash.Internal;

namespace FastHash
{
    public static partial class HashGenerator
    {  
        public static Hash32 GenerateHash32(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get32BitHashFunction(), 4);
        }
        
        public static Hash64 GenerateHash64(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get128BitHashFunction(), 8);
        }
        
        public static Hash128 GenerateHash128(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get128BitHashFunction(), 16);
        }
        
        private static byte[] GenerateHash(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction,
            int hashSizeBytes)
        {
            if (hashFunction is null) throw new ArgumentNullException(nameof(hashFunction));

            var blockSizeBytes = hashFunction.BlockSizeBytes;

            if (blockSizeBytes < hashSizeBytes)
                throw Errors.BlockSizeTooSmall(blockSizeBytes, hashSizeBytes);

            var blockCount = bytes.Length / blockSizeBytes;
            var remainder = bytes.Length % blockSizeBytes;
            
            for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                var start = blockIndex * blockSizeBytes;
                var block = bytes.Slice(start, blockSizeBytes);
                hashFunction.AddCompleteBlock(block);
            }

            if (remainder > 0)
                hashFunction.AddRemainder(bytes.Slice(bytes.Length - remainder));

            return hashFunction.GetFinalHashValue();
        }
    }
}