﻿using System.Linq;
using FluentAssertions;
using Xunit;

namespace FastHash.Tests
{
    // These tests use the fixed set of guids from TestGuids since it is possible for there to be collisions if we use
    // completely random guids each time
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ForDifferentInputs_OutputIsDifferent_32Bit()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => i.GenerateHash32().ToString()).ToArray();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void ForDifferentInputs_OutputIsDifferent_64Bit()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => i.GenerateHash64().ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
        
        [Fact]
        public void ForDifferentInputs_OutputIsDifferent_128Bit()
        {
            var inputs = TestGuids.Get().Select(g => g.ToByteArray()).ToArray();

            var outputs = inputs.Select(i => i.GenerateHash128().ToString()).ToList();

            outputs.Distinct().Count().Should().Be(inputs.Length);
        }
    }
}