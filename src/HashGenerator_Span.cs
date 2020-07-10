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
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get32Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 4);
            
            return GenerateHashImpl(bytes, hashFunction);
        }
        
        public static Hash64 GenerateHash64(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get64Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 8);
            
            return GenerateHashImpl(bytes, hashFunction);
        }
        
        public static Hash128 GenerateHash128(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get128Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 16);
            
            return GenerateHashImpl(bytes, hashFunction);
        }
        
        private static byte[] GenerateHashImpl(
            ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction)
        {
            var blockSizeBytes = hashFunction.BlockSizeBytes;
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