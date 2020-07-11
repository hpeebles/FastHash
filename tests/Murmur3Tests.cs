using System.Text;
using FluentAssertions;
using Xunit;

namespace FastHash.Tests
{
    // Expected hash values taken from http://murmurhash.shorelabs.com/
    public class Murmur3Tests
    {
        [Theory]
        [InlineData("12345", 329585043)]
        [InlineData("AAAAAAAAAA", 1878927834)]
        [InlineData("qwertyuiopasdfghjklzxcvbnm", 1360800970)]
        [InlineData("1234567890abcdefghijklmnopqrstuvwxyz", 1312366228)]
        public void GenerateHash32(string text, int expectedHash)
        {
            var bytes = Encoding.ASCII.GetBytes(text);

            var hash = HashGenerator.GenerateHash32(bytes, Murmur3.Get32BitHashFunction());

            hash.AsInt32().Should().Be(expectedHash);
        }
        
        [Theory]
        [InlineData("12345", "20F83A176B21DFCBF13C5C41325CA9F4")]
        [InlineData("AAAAAAAAAA", "C9F0F61D47424F2F20CB32D57F6EC3DB")]
        [InlineData("qwertyuiopasdfghjklzxcvbnm", "F1EB24425D592E0DC12C5BE4A2C3F608")]
        [InlineData("1234567890abcdefghijklmnopqrstuvwxyz", "0B0D643149D36788800E6FB0060D9603")]
        public void GenerateHash128(string text, string expectedHash)
        {
            var bytes = Encoding.ASCII.GetBytes(text);

            var hash = HashGenerator.GenerateHash128(bytes, Murmur3.Get128BitHashFunction());

            hash.ToString().Should().Be(expectedHash);
        }
    }
}