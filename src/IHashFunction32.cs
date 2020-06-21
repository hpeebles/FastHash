using System;

namespace FastHash
{
    public interface IHashFunction32
    {
        uint Seed { get; }
        uint AddBlock(uint hash, uint nextBlock, long blockIndex);
        uint AddRemainder(uint hash, ReadOnlySpan<byte> remainder, long blockIndex);
        uint Finalize(uint hash, long lengthBytes);
    }
}