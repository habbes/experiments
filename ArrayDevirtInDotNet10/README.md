# Test array enumerator devirtualization performance in .NET 10 Preview

Resources:

- [.NET 10 Preview 1 Announcement](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-1/)
- [Array Interface Method Devirtualization Overview](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview1/runtime.md#array-interface-method-devirtualization)
- [De-abstraction in .NET 10](https://github.com/dotnet/runtime/issues/108913)

To run the benchmarks:

```dotnetcli
dotnet run -c Release -f net10.0 --runtimes net8.0 net10.0
```

Benchmark results:

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3194)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100-preview.1.25120.13
  [Host]     : .NET 10.0.0 (10.0.25.8005), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 10.0.0 (10.0.25.8005), X64 RyuJIT AVX-512F+CD+BW+DQ+VL


| Method                                | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| RunIntSumWithFor                      |   506.6 ns |  12.54 ns |  36.98 ns |   505.3 ns |  1.01 |    0.10 |      - |         - |          NA |
| RunIntSumWithForeach                  |   508.2 ns |  17.00 ns |  50.13 ns |   513.7 ns |  1.01 |    0.12 |      - |         - |          NA |
| RunIntSumWithFor_List                 |   897.2 ns |  34.80 ns | 102.61 ns |   903.4 ns |  1.78 |    0.24 |      - |         - |          NA |
| RunIntSumWithForeach_List             |   945.3 ns |  46.32 ns | 136.58 ns |   964.3 ns |  1.88 |    0.30 |      - |         - |          NA |
| RunIntSumWithForeachIEnumerable_List  | 3,502.5 ns | 128.68 ns | 379.41 ns | 3,604.6 ns |  6.95 |    0.91 | 0.0076 |      40 B |          NA |
| RunIntSumWithForeachIEnumerable_Array | 2,176.4 ns |  36.78 ns |  32.60 ns | 2,177.5 ns |  4.32 |    0.33 | 0.0038 |      32 B |          NA |
| RunIntSumWithForeach_ArrayCast        |   840.9 ns |  18.52 ns |  54.61 ns |   841.9 ns |  1.67 |    0.16 |      - |         - |          NA |

