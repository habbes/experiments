using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using InlineArrayStructDotnet;
using Microsoft.VSDiagnostics;
using UnitTests;

namespace InlineArrayStructDotnet;

[MemoryDiagnoser]
//[CPUUsageDiagnoser]
//[ShortRunJob]
public class Benchmarks
{
    [Params(2, 30)]
    public int numBlocks;
    public string input;


    [GlobalSetup]
    public void Setup()
    {
        input = CreateTestString(numBlocks);
    }


    [Benchmark]
    public bool BracketsMatchingWithStack() => BracketsMatcher.AreBracketsBalanced(input, new StandardCharStack());

    [Benchmark]
    public bool BracketsMatchingWithStack_InitializedCapacity() => BracketsMatcher.AreBracketsBalanced(input, new StandardCharStack(input.Length));

    [Benchmark]
    public bool BracketsMatchingWithOptimizedStack() => BracketsMatcher.AreBracketsBalanced(input, new HybridCharStack());

    [Benchmark]
    public bool BracketsMatchingWithOptimizedListStack() => BracketsMatcher.AreBracketsBalanced(input, new HybridListCharStack());

    [Benchmark]
    public bool BracketsMatchingWithArrayStack() => BracketsMatcher.AreBracketsBalanced(input, new HybridArrayCharStack());

    [Benchmark]
    public bool BracketsMatchingWithPooledArrayStack() => BracketsMatcher.AreBracketsBalanced(input, new HybridPooledArrayCharStack());

    [Benchmark]
    public bool BracketsMatchingWithMinPooledArrayStack() => BracketsMatcher.AreBracketsBalanced(input, new HybridPooledMinArrayCharStack());

    /// <summary>
    /// Helper for creating a string of matched brackets
    /// based on the specified number of repeating blocks.
    /// </summary>
    /// <param name="numBlocks">A block is based on opening "{[(" and closing ")]}" brackets, so the total length of the string will be 6 * numBlocks.</param>
    /// <returns></returns>
    static string CreateTestString(int numBlocks)
    {
        const string OpeningBlock = "{[(";
        const string ClosingBlock = ")]}";

        int totalLength = (OpeningBlock.Length + ClosingBlock.Length) * numBlocks;

        // Tip: When the size of the string is known ahead of time,
        // using string.Create avoids unncessary allocations and copies,
        // more efficient than new string(char[]...), string.Join, or StringBuilder.
        // See: https://learn.microsoft.com/en-us/dotnet/api/system.string.create?#system-string-create-1(system-int32-0-system-buffers-spanaction((system-char-0)))
        return string.Create(totalLength, numBlocks, static (buffer, state) =>
        {
            int pos = 0;
            // generates opening {[({[(... repeatedly numBlocks times
            for (int i = 0; i < state; i++)
            {
                OpeningBlock.AsSpan().CopyTo(buffer[pos..]);
                pos += OpeningBlock.Length;
            }

            // generates closing )]})... repeatedly numBlocks times
            for (int i = 0; i < state; i++)
            {
                ClosingBlock.AsSpan().CopyTo(buffer[pos..]);
                pos += ClosingBlock.Length;
            }
        });
    }

}