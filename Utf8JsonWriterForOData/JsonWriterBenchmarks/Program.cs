using BenchmarkDotNet.Running;
using System;
using System.Threading.Tasks;

namespace JsonWriterBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            }
            else
            {
                // sanity checks that to debug benchmark
                DebugBenchmark().Wait();
            }
        }

        async static Task DebugBenchmark()
        {
            Console.WriteLine(
                "WARN: This is not running benchmarks, just testing the code for debugging purposes, to run the benchmarks make sure to pass CLI args like --filter=*");
            Benchmarks benchmarks = new();

            benchmarks.SetupStreams();
            await benchmarks.ODataMessageWriterUtf8_WriteToFile();
            
        }
    }
}
