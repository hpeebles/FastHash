using System;

namespace FastHash
{
    public interface IHashFunction
    {
        int BlockSizeBytes { get; }
        void AddCompleteBlock(ReadOnlySpan<byte> block);
        void AddRemainder(ReadOnlySpan<byte> remainder);
        byte[] GetFinalHashValue();
    }
}