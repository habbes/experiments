using BenchmarkDotNet;
using BenchmarkDotNet.Running;
using JsonWriterPerf;

BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<Benchmarks>();