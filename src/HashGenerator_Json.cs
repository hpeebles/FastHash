using System.Runtime.CompilerServices;
using System.Text.Json;
using FastHash.Internal;

namespace FastHash
{
    public static partial class HashGenerator
    {
        public static Hash32 GenerateJsonHash32(
            object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get32BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 4)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 4);
            
            return GenerateJsonHashImpl(obj, hashFunction, writerOptions, serializerOptions);
        }
     
        public static Hash64 GenerateJsonHash64(
            object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 8)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 8);
            
            return GenerateJsonHashImpl(obj, hashFunction, writerOptions, serializerOptions);
        }
        
        public static Hash128 GenerateJsonHash128(
            object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 16)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 16);

            return GenerateJsonHashImpl(obj, hashFunction, writerOptions, serializerOptions);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] GenerateJsonHashImpl(
            object obj,
            IHashFunction hashFunction,
            JsonWriterOptions writerOptions,
            JsonSerializerOptions serializerOptions)
        {
            var hashWriter = new HashWriter(hashFunction);

            JsonSerializer.Serialize(new Utf8JsonWriter(hashWriter, writerOptions), obj, serializerOptions);

            return hashWriter.GetResult();
        }
    }
}