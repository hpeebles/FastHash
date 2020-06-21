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
            IHashFunction32 hashFunction,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            var hashWriter = new HashWriter32(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return (int)hashWriter.GetResult();
        }
        
        public static int GenerateHash32(
            this byte[] bytes,
            IHashFunction32 hashFunction)
        {
            return GenerateHash32(new ReadOnlySpan<byte>(bytes), hashFunction);
        }
        
        public static int GenerateHash32(
            this Span<byte> bytes,
            IHashFunction32 hashFunction)
        {
            return GenerateHash32((ReadOnlySpan<byte>)bytes, hashFunction);
        }

        public static int GenerateHash32(
            this ReadOnlySpan<byte> bytes,
            IHashFunction32 hashFunction)
        {
            if (hashFunction is null) throw new ArgumentNullException(nameof(hashFunction));
            
            var hash = hashFunction.Seed;
            var blocks = bytes.Length >> 2;
            var remainder = bytes.Length & 3;

            for (var blockIndex = 0; blockIndex < blocks; blockIndex++)
            {
                var start = blockIndex * 4;
                var block = bytes.Slice(start, 4);
                hash = hashFunction.AddBlock(hash, MemoryMarshal.Read<uint>(block), blockIndex);
            }

            if (remainder > 0)
                hash = hashFunction.AddRemainder(hash, bytes.Slice(bytes.Length - remainder), bytes.Length);

            return (int)hashFunction.Finalize(hash, bytes.Length);
        }
    }
}