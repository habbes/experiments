using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace DotNetSIMD;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[DisassemblyDiagnoser(printSource: true, maxDepth: 3)]
//[SimpleJob(targetCount: = 1)]
public class Benchmarks
{
    private int[]? A;
    private int[]? B;
    private int[]? C;

    [Params(0x1000, 0x10000, 0x100000)]
    public int dataSize;

    [GlobalSetup]
    public void Setup()
    {
        A = new int[dataSize];
        B = new int[dataSize];
        C = new int[dataSize];

        Random generator = new Random(1);
        for (int i = 0; i < dataSize; i++)
        {
            A[i] = generator.Next(0, 10);
            B[i] = generator.Next(0, 10);
            C[i] = 0;
        }
    }


    [BenchmarkCategory("MemberWiseSum"), Benchmark(Baseline = true)]
    public void MemberWiseSumScalar() => Compute.MemberWiseSumScalar(A, B, C);

    [BenchmarkCategory("MemberWiseSum"), Benchmark]
    public void MemberWiseSumSIMD() => Compute.MemberWiseSumSIMD(A, B, C);

    [BenchmarkCategory("ArraySum"), Benchmark(Baseline = true)]
    public void ArraySumSalar() => Compute.ArraySumScalar(A);


    [BenchmarkCategory("ArraySum"), Benchmark]
    public void ArraySumSIMD() => Compute.ArraySumSIMD(A);

}
