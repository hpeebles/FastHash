using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastHash.Internal
{
    internal sealed class HashWriter32 : IBufferWriter<byte>
    {
        private const int BlockSizeBytes = 4;
        private readonly IHashFunction32 _hashFunction;
        private byte[] _byteArray;
        private uint _hash;
        private int _currentByteIndex;
        private int _byteIndexProcessedUpTo;
        private long _blockIndex;
        private long _totalBytesProcessed;
        
        public HashWriter32(IHashFunction32 hashFunction)
        {
            _hashFunction = hashFunction;
            _byteArray = Array.Empty<byte>();
            _hash = hashFunction.Seed;
        }

        public void Advance(int count)
        {
            _currentByteIndex += count;

            while (_currentByteIndex >= _byteIndexProcessedUpTo + BlockSizeBytes)
            {
                var nextBlock = MemoryMarshal.Read<uint>(_byteArray.AsSpan(_byteIndexProcessedUpTo, BlockSizeBytes));
                _hash = _hashFunction.AddBlock(_hash, nextBlock, _blockIndex++);
                _byteIndexProcessedUpTo += BlockSizeBytes;
                _totalBytesProcessed += BlockSizeBytes;
            }
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (!CheckCapacity(sizeHint))
                EnsureCapacity(sizeHint);

            return _byteArray.AsMemory(_currentByteIndex);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            if (!CheckCapacity(sizeHint))
                EnsureCapacity(sizeHint);

            return _byteArray.AsSpan(_currentByteIndex);
        }

        public uint GetResult()
        {
            var bytesRemaining = _currentByteIndex - _byteIndexProcessedUpTo;

            if (bytesRemaining > 0)
            {
                var remainder = _byteArray.AsSpan().Slice(_byteIndexProcessedUpTo, bytesRemaining);
                _hash = _hashFunction.AddRemainder(_hash, remainder, _blockIndex);
                _totalBytesProcessed += bytesRemaining;
            }

            _hash = _hashFunction.Finalize(_hash, _totalBytesProcessed);
            
            var byteArray = _byteArray;
            _byteArray = null;
            ArrayPool<byte>.Shared.Return(byteArray);

            return _hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CheckCapacity(int sizeRequested) => _byteArray.Length - _currentByteIndex >= sizeRequested;

        private void EnsureCapacity(int sizeRequested)
        {
            var currentArray = _byteArray;
            var sizeWaitingToBeProcessed = _currentByteIndex - _byteIndexProcessedUpTo;
            var sizeNeeded = sizeRequested + sizeWaitingToBeProcessed;

            if (sizeNeeded <= currentArray.Length)
            {
                Array.Copy(currentArray, _byteIndexProcessedUpTo, currentArray, 0, sizeWaitingToBeProcessed);
            }
            else
            {
                var newArray = ArrayPool<byte>.Shared.Rent(sizeNeeded);
                _byteArray = newArray;
                
                if (sizeWaitingToBeProcessed > 0)
                    Array.Copy(currentArray, _byteIndexProcessedUpTo, newArray, 0, sizeWaitingToBeProcessed);
                
                ArrayPool<byte>.Shared.Return(currentArray);
            }

            _byteIndexProcessedUpTo = 0;
            _currentByteIndex = sizeWaitingToBeProcessed;
        }
    }
}