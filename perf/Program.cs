using System;
using System.Linq;

namespace FastHash.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarkId = GetBenchmarkId(args);

            switch (benchmarkId)
            {
                case 0:
                    HashGenerator_GenerateHash.Run();
                    break;
                default:
                    Console.WriteLine("No benchmarks found with Id - " + benchmarkId);
                    break;
            }
        }

        private static int GetBenchmarkId(string[] args)
        {
            if (args is null || !args.Any() || !Int32.TryParse(args[0], out var id))
                return 0;

            return id;
        }
    }
}