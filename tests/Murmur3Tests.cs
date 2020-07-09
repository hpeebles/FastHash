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

            var hash = HashGenerator.GenerateHash32(bytes, Murmur3.Get32BitHashFunction());

            hash.AsInt32().Should().Be(expectedHash);
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
            
            var bytesHash = HashGenerator.GenerateHash32(bytes, Murmur3.Get32BitHashFunction());

            var jsonHash = HashGenerator.GenerateJsonHash32(obj, Murmur3.Get32BitHashFunction());
            
            jsonHash.AsInt32().Should().Be(bytesHash.AsInt32());
        }
        
        // Expected hash values taken from http://murmurhash.shorelabs.com/
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
        
        [Theory]
        [InlineData(123, "abc")]
        [InlineData(0, null)]
        [InlineData(1111111111, "AAAAAAAAAA")]
        [InlineData(Int32.MaxValue, "1234567890abcdefghijklmnopqrstuvwxyz")]
        public void GenerateJsonHash128(int a, string b)
        {
            var obj = new TestClass { A = a, B = b };

            var json = JsonSerializer.Serialize(obj);
            var bytes = Encoding.ASCII.GetBytes(json);
            
            var bytesHash = HashGenerator.GenerateHash128(bytes, Murmur3.Get128BitHashFunction());

            var jsonHash = HashGenerator.GenerateJsonHash128(obj, Murmur3.Get128BitHashFunction());
            
            jsonHash.AsGuid().Should().Be(bytesHash.AsGuid());
        }

        private class TestClass
        {
            public int A { get; set; }
            public string B { get; set; }
        }
    }
}