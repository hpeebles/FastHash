using System;
using System.Text.Json;
using FastHash.Internal;

namespace FastHash
{
    public static class ObjectExtensions
    {
        #region 32Bit
        public static Hash32 GenerateJsonHash32(
            this object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get32BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 4)
                throw ThrowBlockSizeTooSmallException(hashFunction.BlockSizeBytes, 4);
            
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash32 GenerateHash32(
            this byte[] bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash32(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash32 GenerateHash32(
            this Span<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash32((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash32 GenerateHash32(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get32BitHashFunction(), 4);
        }
        #endregion 32Bit

        #region 64Bit
        public static Hash64 GenerateJsonHash64(
            this object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 8)
                throw ThrowBlockSizeTooSmallException(hashFunction.BlockSizeBytes, 8);
            
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash64 GenerateHash64(
            this byte[] bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash64(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash64 GenerateHash64(
            this Span<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash64((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash64 GenerateHash64(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get128BitHashFunction(), 8);
        }
        #endregion 64Bit
        
        #region 128Bit
        public static Hash128 GenerateJsonHash128(
            this object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 16)
                throw ThrowBlockSizeTooSmallException(hashFunction.BlockSizeBytes, 16);

            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash128 GenerateHash128(
            this byte[] bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash128(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash128 GenerateHash128(
            this Span<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash128((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash128 GenerateHash128(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction = null)
        {
            return GenerateHash(bytes, hashFunction ?? Murmur3.Get128BitHashFunction(), 16);
        }
        #endregion 128Bit
        
        private static byte[] GenerateHash(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction,
            int hashSizeBytes)
        {
            if (hashFunction is null) throw new ArgumentNullException(nameof(hashFunction));

            var blockSizeBytes = hashFunction.BlockSizeBytes;

            if (blockSizeBytes < hashSizeBytes)
                throw ThrowBlockSizeTooSmallException(blockSizeBytes, hashSizeBytes);
            
            var powerOf2 = blockSizeBytes switch
            {
                4 => 2,
                8 => 3,
                16 => 4,
                _ => throw new ArgumentException("Block size not supported - " + hashFunction.BlockSizeBytes, nameof(hashFunction))
            };

            var blockCount = bytes.Length >> powerOf2;
            var remainder = bytes.Length & (blockSizeBytes - 1);
            
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

        private static ArgumentException ThrowBlockSizeTooSmallException(int blockSize, int requiredSize)
        {
            return new ArgumentException($"Block size too small: {blockSize}. Required size: {requiredSize}");
        }
    }
}