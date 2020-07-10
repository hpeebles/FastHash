using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FastHash.Internal;

namespace FastHash
{
    public static partial class HashGenerator
    {
        public static async Task<Hash32> GenerateHash32Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get32BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 4)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 4);

            return await GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken).ConfigureAwait(false);
        }
        
        public static async Task<Hash64> GenerateHash64Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 8)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 8);

            return await GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken).ConfigureAwait(false);
        }
        
        public static async Task<Hash128> GenerateHash128Async(
            Stream stream,
            IHashFunction hashFunction = null,
            int bufferSize = 1024,
            CancellationToken cancellationToken = default)
        {
            if (hashFunction is null)
                hashFunction = Murmur3.Get128BitHashFunction();
            else if (hashFunction.BlockSizeBytes < 16)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, 16);

            return await GenerateHashAsyncImpl(stream, hashFunction, bufferSize, cancellationToken).ConfigureAwait(false);
        }
        
        private static async Task<byte[]> GenerateHashAsyncImpl(
            Stream stream,
            IHashFunction hashFunction,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            var hashWriter = new HashWriter(hashFunction);

            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = hashWriter.GetMemory(bufferSize);
                
                var bytesRead = await stream
                    .ReadAsync(buffer, cancellationToken)
                    .ConfigureAwait(false);

                if (bytesRead == 0)
                    break;
                
                hashWriter.Advance(bytesRead);
            }
            
            return hashWriter.GetResult();
        }
    }
}