using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FastHash.Internal;

namespace FastHash
{
    public static partial class HashGenerator
    {
        public static async ValueTask<Hash32> GenerateHash32Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get32Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 4);

            var getBytesTask = GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken);

            return getBytesTask.IsCompleted
                ? getBytesTask.Result
                : await getBytesTask.ConfigureAwait(false);
        }
        
        public static async ValueTask<Hash64> GenerateHash64Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get64Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 8);

            var getBytesTask = GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken);

            return getBytesTask.IsCompleted
                ? getBytesTask.Result
                : await getBytesTask.ConfigureAwait(false);
        }
        
        public static async ValueTask<Hash128> GenerateHash128Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = DefaultHashFunction.Get128Bit();
            else 
                HashFunctionValidator.Validate(hashFunction, 16);

            var getBytesTask = GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken);

            return getBytesTask.IsCompleted
                ? getBytesTask.Result
                : await getBytesTask.ConfigureAwait(false);
        }
        
        private static async ValueTask<byte[]> GenerateHashAsyncImpl(
            Stream stream,
            IHashFunction hashFunction,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            var hashWriter = new HashWriter(hashFunction);

            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = hashWriter.GetMemory(bufferSize);
                
                var readBytesTask = stream.ReadAsync(buffer, cancellationToken);

                var bytesRead = readBytesTask.IsCompleted
                    ? readBytesTask.Result
                    : await readBytesTask.ConfigureAwait(false);
                
                if (bytesRead == 0)
                    break;
                
                hashWriter.Advance(bytesRead);
            }
            
            return hashWriter.GetResult();
        }
    }
}