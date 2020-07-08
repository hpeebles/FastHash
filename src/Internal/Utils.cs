using System.Runtime.CompilerServices;

namespace FastHash.Internal
{
    internal static class Utils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int BlockCount, int Remainder) GetBlockCountAndRemainder(int lengthBytes, int blockSizeBytes)
        {
            int blockCount;
            int remainder;
            switch (blockSizeBytes)
            {
                case 4:
                    blockCount = lengthBytes >> 2;
                    remainder = lengthBytes & 3;
                    break;
                
                case 8:
                    blockCount = lengthBytes >> 3;
                    remainder = lengthBytes & 7;
                    break;
                
                case 16:
                    blockCount = lengthBytes >> 4;
                    remainder = lengthBytes & 15;
                    break;
                
                case 32:
                    blockCount = lengthBytes >> 5;
                    remainder = lengthBytes & 31;
                    break;
                
                default:
                    blockCount = lengthBytes / blockSizeBytes;
                    remainder = lengthBytes % blockSizeBytes;
                    break;
            }

            return (blockCount, remainder);
        }
    }
}