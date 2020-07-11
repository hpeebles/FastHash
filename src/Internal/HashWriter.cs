using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace FastHash.Internal
{
    internal sealed class HashWriter : IBufferWriter<byte>
    {
        private readonly IHashFunction _hashFunction;
        private readonly int _blockSizeBytes;
        private byte[] _buffer;
        private int _currentByteIndex;
        private int _byteIndexProcessedUpTo;

        public HashWriter(IHashFunction hashFunction)
        {
            _hashFunction = hashFunction;
            _blockSizeBytes = hashFunction.BlockSizeBytes;
            _buffer = Array.Empty<byte>();
        }
        
        public void Advance(int count)
        {
            _currentByteIndex += count;

            while (_currentByteIndex >= _byteIndexProcessedUpTo + _blockSizeBytes)
            {
                var block = _buffer.AsSpan(_byteIndexProcessedUpTo, _blockSizeBytes);
                
                _hashFunction.AddCompleteBlock(block);

                _byteIndexProcessedUpTo += _blockSizeBytes;
            }
        }
        
        public byte[] GetResult()
        {
            var bytesRemaining = _currentByteIndex - _byteIndexProcessedUpTo;

            if (bytesRemaining > 0)
            {
                var remainder = _buffer.AsSpan().Slice(_byteIndexProcessedUpTo, bytesRemaining);
                
                _hashFunction.AddRemainder(remainder);
            }

            var hash = _hashFunction.GetFinalHashValue();
            
            var buffer = _buffer;
            _buffer = Array.Empty<byte>();
            ArrayPool<byte>.Shared.Return(buffer);

            return hash;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (!CheckCapacity(sizeHint))
                EnsureCapacity(sizeHint);

            return _buffer.AsMemory(_currentByteIndex);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            if (!CheckCapacity(sizeHint))
                EnsureCapacity(sizeHint);

            return _buffer.AsSpan(_currentByteIndex);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CheckCapacity(int sizeRequested) => _buffer.Length - _currentByteIndex >= sizeRequested;

        private void EnsureCapacity(int sizeRequested)
        {
            var buffer = _buffer;
            var sizeWaitingToBeProcessed = _currentByteIndex - _byteIndexProcessedUpTo;
            var sizeNeeded = sizeRequested + sizeWaitingToBeProcessed;

            if (sizeNeeded <= buffer.Length)
            {
                Array.Copy(buffer, _byteIndexProcessedUpTo, buffer, 0, sizeWaitingToBeProcessed);
            }
            else
            {
                var newBuffer = ArrayPool<byte>.Shared.Rent(sizeNeeded);
                _buffer = newBuffer;
                
                if (sizeWaitingToBeProcessed > 0)
                    Array.Copy(buffer, _byteIndexProcessedUpTo, newBuffer, 0, sizeWaitingToBeProcessed);
                
                ArrayPool<byte>.Shared.Return(buffer);
            }

            _byteIndexProcessedUpTo = 0;
            _currentByteIndex = sizeWaitingToBeProcessed;
        }
    }
}