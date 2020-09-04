using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FastHash.Tests
{
    // These tests use the fixed set of guids from TestGuids since it is possible for there to be collisions if we use
    // completely random guids each time
    public class HashGeneratorTests
    {
        [Fact]
        public void GenerateHash32_ForDifferentByteArrayInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash32(i).ToString()).ToArray();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void GenerateHash64_ForDifferentByteArrayInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash64(i).ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void GenerateHash128_ForDifferentByteArrayInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash128(i).ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void GenerateHash32_ForDifferentStringInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToString()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash32(i).ToString()).ToArray();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void GenerateHash64_ForDifferentStringInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToString()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash64(i).ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void GenerateHash128_ForDifferentStringInputs_ProducesDifferentOutputs()
        {
            var inputs = TestGuids.Get().Select(g => g.ToString()).ToArray();

            var outputs = inputs.Select(i => HashGenerator.GenerateHash128(i).ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public async Task GenerateHash32AndGenerateHash32Async_SameArrayOfBytes_ProduceTheSameHash()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            foreach (var input in inputs)
            {
                var hash1 = HashGenerator.GenerateHash32(input);
                var hash2 = await HashGenerator.GenerateHash32Async(new MemoryStream(input));

                hash1.ToString().Should().Be(hash2.ToString());
            }
        }
        
        [Fact]
        public async Task GenerateHash64AndGenerateHash64Async_SameArrayOfBytes_ProduceTheSameHash()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            foreach (var input in inputs)
            {
                var hash1 = HashGenerator.GenerateHash64(input);
                var hash2 = await HashGenerator.GenerateHash64Async(new MemoryStream(input));

                hash1.ToString().Should().Be(hash2.ToString());
            }
        }
        
        [Fact]
        public async Task GenerateHash128AndGenerateHash128Async_SameArrayOfBytes_ProduceTheSameHash()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            foreach (var input in inputs)
            {
                var hash1 = HashGenerator.GenerateHash128(input);
                var hash2 = await HashGenerator.GenerateHash128Async(new MemoryStream(input));

                hash1.ToString().Should().Be(hash2.ToString());
            }
        }

        [Fact]
        public void GenerateHash32AndGenerateJsonHash32_SameSourceObject_ProduceTheSameHash()
        {
            var guids = TestGuids.Get();

            for (var i = 0; i < guids.Length; i++)
            {
                var obj = new TestClass { A = i, B = new String(i.ToString()[0], i), C = guids[i] };

                var json = JsonSerializer.Serialize(obj);
                var bytes = Encoding.ASCII.GetBytes(json);

                var bytesHash = HashGenerator.GenerateHash32(bytes);
                var jsonHash = HashGenerator.GenerateJsonHash32(obj);

                jsonHash.AsInt32().Should().Be(bytesHash.AsInt32());
            }
        }

        [Fact]
        public void GenerateHash64AndGenerateJsonHash64_SameSourceObject_ProduceTheSameHash()
        {
            var guids = TestGuids.Get();

            for (var i = 0; i < guids.Length; i++)
            {
                var obj = new TestClass { A = i, B = new String(i.ToString()[0], i), C = guids[i] };

                var json = JsonSerializer.Serialize(obj);
                var bytes = Encoding.ASCII.GetBytes(json);

                var bytesHash = HashGenerator.GenerateHash64(bytes);
                var jsonHash = HashGenerator.GenerateJsonHash64(obj);

                jsonHash.AsInt64().Should().Be(bytesHash.AsInt64());
            }
        }

        [Fact]
        public void GenerateHash128AndGenerateJsonHash128_SameSourceObject_ProduceTheSameHash()
        {
            var guids = TestGuids.Get();

            for (var i = 0; i < guids.Length; i++)
            {
                var obj = new TestClass { A = i, B = new String(i.ToString()[0], i), C = guids[i] };

                var json = JsonSerializer.Serialize(obj);
                var bytes = Encoding.ASCII.GetBytes(json);

                var bytesHash = HashGenerator.GenerateHash128(bytes);
                var jsonHash = HashGenerator.GenerateJsonHash128(obj);

                jsonHash.ToString().Should().Be(bytesHash.ToString());
            }
        }

        private class TestClass
        {
            public int A { get; set; }
            public string? B { get; set; }
            public Guid C { get; set; }
        }
    }
}