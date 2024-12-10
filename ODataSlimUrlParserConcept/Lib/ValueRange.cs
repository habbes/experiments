namespace Lib;

public record struct ValueRange(int Start, int Length)
{
    public readonly ReadOnlySpan<char> GetSpan(ReadOnlySpan<char> source) => source.Slice(Start, Length);
    public readonly ReadOnlyMemory<char> GetMemory(ReadOnlyMemory<char> source) => source.Slice(Start, Length);
}