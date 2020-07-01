using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using FastHash.Internal;

namespace FastHash
{
    public static class ObjectExtensions
    {
        public static int GenerateJsonHash32(
            this object obj,
            IHashFunction hashFunction,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return MemoryMarshal.Read<int>(hashWriter.GetResult());
        }
        
        public static int GenerateHash32(
            this byte[] bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash32(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static int GenerateHash32(
            this Span<byte> bytes,
            IHashFunction hashFunction)
        {
            return GenerateHash32((ReadOnlySpan<byte>)bytes, hashFunction);
        }

        public static int GenerateHash32(
            this ReadOnlySpan<byte> bytes,
            IHashFunction hashFunction)
        {
            if (hashFunction is null) throw new ArgumentNullException(nameof(hashFunction));

            var powerOf2 = hashFunction.BlockSizeBytes switch
            {
                4 => 2,
                8 => 3,
                16 => 4,
                _ => throw new ArgumentException("HashFunction block size not supported - " + hashFunction.BlockSizeBytes, nameof(hashFunction))
            };

            var blockCount = bytes.Length >> powerOf2;
            var remainder = bytes.Length & (hashFunction.BlockSizeBytes - 1);
            
            for (var blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                var start = blockIndex * 4;
                var block = bytes.Slice(start, 4);
                hashFunction.AddCompleteBlock(block);
            }

            if (remainder > 0)
                hashFunction.AddRemainder(bytes.Slice(bytes.Length - remainder));

            return MemoryMarshal.Read<int>(hashFunction.GetFinalHashValue());
        }
    }
}