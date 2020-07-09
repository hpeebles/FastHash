using System;

namespace FastHash.Internal
{
    internal static class Errors
    {
        public static ArgumentException BlockSizeTooSmall(int blockSize, int requiredSize)
        {
            return new ArgumentException($"Block size too small: {blockSize}. Required size: {requiredSize}");
        }
    }
}