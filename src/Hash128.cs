using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FastHash
{
    public readonly struct Hash128
    {
        private readonly ReadOnlyMemory<byte> _bytes;

        public Hash128(ReadOnlyMemory<byte> bytes)
        {
            if (bytes.Length != 16)
            {
                if (bytes.Length < 16)
                    throw new ArgumentException("Bytes should have length of at least 16 but has length " + bytes.Length, nameof(bytes));

                if (bytes.Length > 16)
                    bytes = bytes.Slice(0, 16);
            }

            _bytes = bytes;
        }

        public ReadOnlySpan<byte> AsSpan() => _bytes.Span;
        public Guid AsGuid() => MemoryMarshal.Read<Guid>(_bytes.Span);

        public Hash64 High => new Hash64(_bytes.Slice(0, 8));
        public Hash64 Low => new Hash64(_bytes.Slice(8, 8));
        
        public override string ToString()
        {
            var builder = new StringBuilder(32);
            builder.Append(High.ToString());
            builder.Append(Low.ToString());
            return builder.ToString();
        }

        public static implicit operator Hash128(byte[] bytes) => new Hash128(bytes);
        public static implicit operator ReadOnlySpan<byte>(Hash128 hashValue) => hashValue.AsSpan();
        public static implicit operator Guid(Hash128 hashValue) => hashValue.AsGuid();

        public void Deconstruct(out Hash64 high, out Hash64 low)
        {
            high = High;
            low = Low;
        }
    }
}