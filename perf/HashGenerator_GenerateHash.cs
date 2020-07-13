using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace FastHash.Benchmarks
{
    public class HashGenerator_GenerateHash
    {
        private readonly byte[] _bytes_1KB;
        private readonly byte[] _bytes_1MB;

        public HashGenerator_GenerateHash()
        {
            _bytes_1KB = Enumerable
                .Range(0, 64)
                .SelectMany(_ => Guid.NewGuid().ToByteArray())
                .ToArray();
            
            _bytes_1MB = Enumerable
                .Range(0, 64 * 1024)
                .SelectMany(_ => Guid.NewGuid().ToByteArray())
                .ToArray();
        }
        
        [ParamsAllValues]
        public InputSize InputSize { get; set; }
        
        public static void Run()
        {
#if DEBUG
            var runner = new HashGenerator_GenerateHash();
        
            runner.GenerateHash32();
            runner.GenerateHash64();
            runner.GenerateHash128();
#else
            BenchmarkRunner.Run<HashGenerator_GenerateHash>(ManualConfig
                .Create(DefaultConfig.Instance)
                .AddJob(Job.MediumRun.WithLaunchCount(1))
                .AddDiagnoser(MemoryDiagnoser.Default));
#endif
        }

        [Benchmark]
        public Hash32 GenerateHash32()
        {
            return HashGenerator.GenerateHash32(InputSize == InputSize.OneKB ? _bytes_1KB : _bytes_1MB);
        }

        [Benchmark]
        public Hash64 GenerateHash64()
        {
            return HashGenerator.GenerateHash64(InputSize == InputSize.OneKB ? _bytes_1KB : _bytes_1MB);
        }
        
        [Benchmark]
        public Hash128 GenerateHash128()
        {
            return HashGenerator.GenerateHash128(InputSize == InputSize.OneKB ? _bytes_1KB : _bytes_1MB);
        }
    }
}
/*
// * Summary *

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.900 (1903/May2019Update/19H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.300-preview-015135
  [Host]    : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  MediumRun : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Job=MediumRun  IterationCount=15  LaunchCount=1
WarmupCount=10

|          Method | InputSize |         Mean |       Error |      StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |---------- |-------------:|------------:|------------:|-------:|------:|------:|----------:|
|  GenerateHash32 |     OneKB |     852.9 ns |     7.90 ns |     7.39 ns | 0.0048 |     - |     - |      32 B |
|  GenerateHash64 |     OneKB |     348.3 ns |     4.49 ns |     4.20 ns | 0.0062 |     - |     - |      40 B |
| GenerateHash128 |     OneKB |     349.6 ns |     8.33 ns |     7.79 ns | 0.0062 |     - |     - |      40 B |
|  GenerateHash32 |     OneMB | 821,220.6 ns | 6,976.06 ns | 6,525.41 ns |      - |     - |     - |      32 B |
|  GenerateHash64 |     OneMB | 318,834.1 ns | 6,539.74 ns | 6,117.28 ns |      - |     - |     - |      43 B |
| GenerateHash128 |     OneMB | 299,751.7 ns | 8,463.82 ns | 7,917.06 ns |      - |     - |     - |      43 B |
*/