# Proof-of-concept of a lightweight, memory efficient replacement for `ODataUriParser`

This PoC focuses on query options, and especially expression parsing since that's more complex that parsing path segments or even a select clause. If we can get something that works
for expressions, than we can extend it to support other types of clauses.

## Code organization

- [`Lib`](./Lib): The project implementing the lightweight OData URI parser.
- [`Demo`](./Demo): Playground project I use for testing things out.
- [`SlimParserTests`](./SlimParserTests/`): Unit tests to verify the correctness of the PoC implementation as I go.
- [`Benchmarks`](./Benchmarks): Benchmarks to measure and compare the performance as I go.

## Checkpoint 1: Allocation free lexer

The first checkpoint is an allocation-free tokenizer/lexer. It's implemented as ref struct and takes a `ReadOnlySpan<char>` as input.

The lexer has a simple, forward-only API:

```csharp
var lexer = new ExpressionLexer(sourceSpan);

while (lexer.Read())
{
    if (lexer.Token.Kind == ExpressionTokenKind.Identifier)
    {
        // handle token
    }
    else if (otherKind)
    {
        // ...
    }
}
```

Before I continue to build the syntactic parser, let me compare performance with existing implementation to see how much headroom I have:

```md
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
```

The benchmarks compare parsing the following filter expression: "category eq 'electronics' or price gt 100".

| Method                                   | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------------------------------- |-------------:|-----------:|-----------:|-------:|----------:|
| ParseExpression_ODataQueryOptionParser   | 23,349.92 ns | 454.424 ns | 622.021 ns | 1.4343 |    6256 B |
| ParseExpression_UriQueryExpressionParser |  2,045.97 ns |  41.704 ns | 120.325 ns | 0.2899 |    1264 B |
| ParseExpression_SlimExpressionLexer      |     90.21 ns |   1.688 ns |   1.579 ns |      - |         - |

- `ODataQueryOptionParser` is a semantic parser. This requires the model and binds IEdmModel type information and semantic context to the expression tree.
- `UriQueryExpressionParser` is a syntactic parser. It parses the expression without requiring the model.
- `SlimExpressionLexer` refers to the lightweight lexer built in this step.

## Checkpoint 2: Lightweight syntactic parser

TODO

## Checkpoint 3 : Lightweight semantic parser

(with IEdmModel-based semantic context and validation)

TODO:
