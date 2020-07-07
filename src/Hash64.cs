using System;
using System.Runtime.InteropServices;

namespace FastHash
{
    public readonly struct Hash64
    {
        private readonly ReadOnlyMemory<byte> _bytes;

        public Hash64(ReadOnlyMemory<byte> bytes)
        {
            if (bytes.Length != 8)
            {
                if (bytes.Length < 8)
                    throw new ArgumentException("Bytes should have length of at least 8 but has length " + bytes.Length, nameof(bytes));

                if (bytes.Length > 8)
                    bytes = bytes.Slice(0, 8);
            }

            _bytes = bytes;
        }

        public ReadOnlySpan<byte> AsSpan() => _bytes.Span;
        public long AsInt64() => MemoryMarshal.Read<long>(_bytes.Span);
        public ulong AsUInt64() => MemoryMarshal.Read<ulong>(_bytes.Span);
        
        public override string ToString() => AsInt64().ToString("X16");

        public static implicit operator Hash64(byte[] bytes) => new Hash64(bytes);
        public static implicit operator ReadOnlySpan<byte>(Hash64 hashValue) => hashValue.AsSpan();
        public static implicit operator long(Hash64 hashValue) => hashValue.AsInt64();
    }
}