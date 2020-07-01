using FastHash.Internal;
using Microsoft.Extensions.ObjectPool;

namespace FastHash
{
    public class Murmur3
    {
        private static readonly ObjectPool<Murmur3_32Bit> Pool32Bit;

        static Murmur3()
        {
            Pool32Bit = new DefaultObjectPool<Murmur3_32Bit>(new DefaultPooledObjectPolicy<Murmur3_32Bit>());
        }
        
        public static IHashFunction Get32Bit() => Pool32Bit.Get();
        internal static void Return(Murmur3_32Bit item) => Pool32Bit.Return(item);
    }
}