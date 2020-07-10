namespace FastHash.Internal
{
    internal static class HashFunctionValidator
    {
        public static void Validate(IHashFunction hashFunction, int hashSizeBytes)
        {
            if (hashFunction.BlockSizeBytes < hashSizeBytes)
                throw Errors.BlockSizeTooSmall(hashFunction.BlockSizeBytes, hashSizeBytes);
        }
    }
}