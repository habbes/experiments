// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(ParserBenchmarks).Assembly).Run(args);