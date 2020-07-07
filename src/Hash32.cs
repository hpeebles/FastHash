using System;
using System.Runtime.InteropServices;

namespace FastHash
{
    public readonly struct Hash32
    {
        private readonly ReadOnlyMemory<byte> _bytes;

        public Hash32(ReadOnlyMemory<byte> bytes)
        {
            if (bytes.Length != 4)
            {
                if (bytes.Length < 4)
                    throw new ArgumentException("Bytes should have length of at least 4 but has length " + bytes.Length, nameof(bytes));

                if (bytes.Length > 4)
                    bytes = bytes.Slice(0, 4);
            }

            _bytes = bytes;
        }

        public ReadOnlySpan<byte> AsSpan() => _bytes.Span;
        public int AsInt32() => MemoryMarshal.Read<int>(_bytes.Span);
        public uint AsUInt32() => MemoryMarshal.Read<uint>(_bytes.Span);

        public static implicit operator Hash32(byte[] bytes) => new Hash32(bytes);
        public static implicit operator ReadOnlySpan<byte>(Hash32 hashValue) => hashValue.AsSpan();
        public static implicit operator int(Hash32 hashValue) => hashValue.AsInt32();
    }
}