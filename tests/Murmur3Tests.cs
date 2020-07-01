using System;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace FastHash.Tests
{
    public class Murmur3Tests
    {
        // Expected hash values taken from http://murmurhash.shorelabs.com/
        [Theory]
        [InlineData("12345", 329585043)]
        [InlineData("AAAAAAAAAA", 1878927834)]
        [InlineData("qwertyuiopasdfghjklzxcvbnm", 1360800970)]
        [InlineData("1234567890abcdefghijklmnopqrstuvwxyz", 1312366228)]
        public void GenerateHash32(string text, int expectedHash)
        {
            var bytes = Encoding.ASCII.GetBytes(text);

            var hash = bytes.GenerateHash32(Murmur3.Get32Bit());

            hash.Should().Be(expectedHash);
        }
        
        [Theory]
        [InlineData(123, "abc")]
        [InlineData(0, null)]
        [InlineData(1111111111, "AAAAAAAAAA")]
        [InlineData(Int32.MaxValue, "1234567890abcdefghijklmnopqrstuvwxyz")]
        public void GenerateJsonHash32(int a, string b)
        {
            var obj = new TestClass { A = a, B = b };

            var json = JsonSerializer.Serialize(obj);
            var bytes = Encoding.ASCII.GetBytes(json);
            
            var bytesHash = bytes.GenerateHash32(Murmur3.Get32Bit());

            var jsonHash = obj.GenerateJsonHash32(Murmur3.Get32Bit());
            
            jsonHash.Should().Be(bytesHash);
        }

        private class TestClass
        {
            public int A { get; set; }
            public string B { get; set; }
        }
    }
}