namespace FastHash.Internal
{
    internal static class DefaultHashFunction
    {
        public static IHashFunction Get32Bit() => Murmur3.Get32BitHashFunction();
        public static IHashFunction Get64Bit() => Murmur3.Get128BitHashFunction();
        public static IHashFunction Get128Bit() => Murmur3.Get128BitHashFunction();
    }
}