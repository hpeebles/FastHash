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
            IHashFunction hashFunction,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash32 GenerateHash32(
            this byte[] bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash32(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash32 GenerateHash32(
            this Span<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash32((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash32 GenerateHash32(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash(bytes, hashFunction, 4);
        }
        #endregion 32Bit

        #region 64Bit
        public static Hash64 GenerateJsonHash64(
            this object obj,
            IHashFunction hashFunction,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash64 GenerateHash64(
            this byte[] bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash64(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash64 GenerateHash64(
            this Span<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash64((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash64 GenerateHash64(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash(bytes, hashFunction, 8);
        }
        #endregion 64Bit
        
        #region 128Bit
        public static Hash128 GenerateJsonHash128(
            this object obj,
            IHashFunction hashFunction,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
        
        public static Hash128 GenerateHash128(
            this byte[] bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash128(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static Hash128 GenerateHash128(
            this Span<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash128((ReadOnlySpan<byte>)bytes, hashFunction);
        }
        
        public static Hash128 GenerateHash128(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash(bytes, hashFunction, 16);
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
                throw new ArgumentException("Block size must be at least as big as the output hash size", nameof(hashFunction));
            
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
    }
}