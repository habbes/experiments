# Proof-of-concept of a lightweight, memory efficient replacement for `ODataUriParser`

This PoC focuses on query options, and especially expression parsing since that's more complex that parsing path segments or even a select clause. If we can get something that works
for expressions, than we can extend it to support other types of clauses.

This implementation assume that we have the full source string in memory, and that it's acceptable to keep the source in memory for the duration of the parsing and processing of
the parsed nodes. (TODO, we'll need to keep the original source string after the semantic parse?)

`ODataQueryOptionsParser` also keeps the source string in memory via the `Uri` input it takes. However, we can discard of the parser after we have the semantic expression tree, and
therefore we no longer need to keep the original source string in memory. But the cost is that we had to allocate many substrings for the individual values

## Code organization

- [`Lib`](./Lib): The project implementing the lightweight OData URI parser.
- [`Demo`](./Demo): Playground project I use for testing things out.
- [`SlimParserTests`](./SlimParserTests/`): Unit tests to verify the correctness of the PoC implementation as I go.
- [`Benchmarks`](./Benchmarks): Benchmarks to measure and compare the performance as I go.

## Checkpoint 1: Allocation free lexer

[**View checkpoint's commit: `b5d4a56`**](https://github.com/habbes/experiments/pull/3/commits/b5d4a567182e688355a695a8b56080a2679a01ee)

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

The benchmarks compare parsing the following filter expression: `"category eq 'electronics' or price gt 100"`.

```md
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
```

| Method                                   | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------------------------------- |-------------:|-----------:|-----------:|-------:|----------:|
| ParseExpression_ODataQueryOptionParser   | 23,349.92 ns | 454.424 ns | 622.021 ns | 1.4343 |    6256 B |
| ParseExpression_UriQueryExpressionParser |  2,045.97 ns |  41.704 ns | 120.325 ns | 0.2899 |    1264 B |
| ParseExpression_SlimExpressionLexer      |     90.21 ns |   1.688 ns |   1.579 ns |      - |         - |

- `ODataQueryOptionParser` is a semantic parser. This requires the model and binds IEdmModel type information and semantic context to the expression tree.
- `UriQueryExpressionParser` is a syntactic parser. It parses the expression without requiring the model.
- `SlimExpressionLexer` refers to the lightweight lexer built in this step.

## Checkpoint 2: Lightweight syntactic parser

This is a temporary result, based on a syntactic parser that does not honor operator precedence. Just keeping here for reference
to see how the perf changes after I implement proper operator precedence support.

| Method                                   | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------------------------------- |-------------:|-----------:|-----------:|-------:|----------:|
| ParseExpression_ODataQueryOptionParser   | 22,792.42 ns | 452.168 ns | 502.583 ns | 1.3733 |    6024 B |
| ParseExpression_UriQueryExpressionParser |  1,689.61 ns |  21.732 ns |  19.265 ns | 0.2918 |    1264 B |
| ParseExpression_SlimQueryParser          |    195.45 ns |   2.091 ns |   2.053 ns | 0.0834 |     360 B |
| ParseExpression_SlimExpressionLexer      |     81.59 ns |   1.269 ns |   1.125 ns |      - |         - |
| QueryRoundTrip_UriQueryExpressionParser  |  1,986.13 ns |  39.501 ns |  40.565 ns | 0.4044 |    1752 B |
| QueryRoundTrip_SlimQueryParser           |    443.36 ns |   8.420 ns |   8.269 ns | 0.1965 |     848 B |

TODO:

## Checkpoint 3 : Lightweight semantic parser

(with IEdmModel-based semantic context and validation)

TODO:
