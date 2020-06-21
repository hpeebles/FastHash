using System.Runtime.CompilerServices;

namespace FastHash.Internal
{
    internal static class BitHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
    }
}