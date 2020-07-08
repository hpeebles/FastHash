using FastHash.Internal;
using Microsoft.Extensions.ObjectPool;

namespace FastHash
{
    public class Murmur3
    {
        private static readonly ObjectPool<Murmur3_32Bit> Pool32Bit;
        private static readonly ObjectPool<Murmur3_128Bit> Pool128Bit;

        static Murmur3()
        {
            Pool32Bit = new DefaultObjectPool<Murmur3_32Bit>(new DefaultPooledObjectPolicy<Murmur3_32Bit>());
            Pool128Bit = new DefaultObjectPool<Murmur3_128Bit>(new DefaultPooledObjectPolicy<Murmur3_128Bit>());
        }
        
        public static IHashFunction Get32BitHashFunction() => Pool32Bit.Get();
        public static IHashFunction Get128BitHashFunction() => Pool128Bit.Get();
        
        internal static void Return(Murmur3_32Bit item) => Pool32Bit.Return(item);
        internal static void Return(Murmur3_128Bit item) => Pool128Bit.Return(item);
    }
}