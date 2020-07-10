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
                hashFunction = DefaultHashFunction.Get32Bit();
            else
                HashFunctionValidator.Validate(hashFunction, 4);
            
            return GenerateJsonHashImpl(obj, hashFunction, writerOptions, serializerOptions);
        }
     
        public static Hash64 GenerateJsonHash64(
            object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get64Bit();
            else
                HashFunctionValidator.Validate(hashFunction, 8);
            
            return GenerateJsonHashImpl(obj, hashFunction, writerOptions, serializerOptions);
        }
        
        public static Hash128 GenerateJsonHash128(
            object obj,
            IHashFunction hashFunction = null,
            JsonWriterOptions writerOptions = default,
            JsonSerializerOptions serializerOptions = default)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get128Bit();
            else
                HashFunctionValidator.Validate(hashFunction, 16);

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