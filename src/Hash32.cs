using System;
using System.Runtime.InteropServices;

namespace FastHash
{
    public readonly struct Hash32
    {
        private readonly byte[] _bytes;

        public Hash32(byte[] bytes)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 4)
                throw new ArgumentException("Bytes should have length 4 but instead has length " + bytes.Length, nameof(bytes));
            
            _bytes = bytes;
        }

        public ReadOnlySpan<byte> AsSpan() => _bytes;
        public int AsInt() => MemoryMarshal.Read<int>(_bytes);

        public static implicit operator Hash32(byte[] bytes) => new Hash32(bytes);
        public static implicit operator byte[](Hash32 hashValue) => hashValue._bytes;
        public static implicit operator int(Hash32 hashValue) => hashValue.AsInt();
    }
}