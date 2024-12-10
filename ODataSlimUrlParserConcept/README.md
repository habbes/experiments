# Proof-of-concept of a lightweight, memory efficient replacement for `ODataUriParser`

This PoC focuses on query options, and especially expression parsing since that's more complex that parsing path segments or even a select clause. If we can get something that works
for expressions, than we can extend it to support other types of clauses
(e.g. select is simpler because it's linear, even when nested, we can read from left to right without having to worry about precedence of previous tokens.)

This implementation assume that we have the full source string in memory, and that it's acceptable to keep the source in memory for the duration of the parsing and processing of
the parsed nodes. (TODO, we'll need to keep the original source string after the semantic parse?)

`ODataQueryOptionsParser` also keeps the source string in memory via the `Uri` input it takes. However, we can discard of the parser after we have the semantic expression tree, and
therefore we no longer need to keep the original source string in memory. But the cost is that we had to allocate many substrings for the individual values.

The results in this doc are not conclusive. This PoC implements a very small subset of the required OData query parsing and validation capabilities. Consequently, we expect less
overhead. The idea however is to demonstrate such a significant gap between the PoC and existing implementation (e.g. 10x difference) to gain confidence that should we
implement a fully-featured version of the parser using the same architecture and techniques proposed here, that we would get something at least 3x better than the existing implementation.

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

[**View checkpoint's commit: `86761cf`**](https://github.com/habbes/experiments/pull/3/commits/86761cf0dd4cd08d857efbd80211bf229d869eb0)

This checkpoint implements a light-weight syntactic parser that generates an expression tree from query expression string.
To minimize memory use, the tree nodes are stored in an array (wrapped inside a custom `NodeList` struct) instead of a recursive tree structures. Nodes use indices
in the list to find their children.

`ExpressionParser.Parse(source)` returns a `SlimQueryNode` which provides APIs for traversing the query DOM. The `SlimQueryNode`
is a lightweight struct that indexes into the internal `ExpressionParser`'s nodes list. This approach is inspired by `JsonDocument` and `JsonElement`'s
design in `System.Text.Json`.

I also added a `ISyntacticTreeHandler` interface to mirror the `ISyntacticVisitor` interface in OData. So the nodes in this PoC are structs and do not
have inheritance, the handler methods specify the name of the node type they're meant to handle, i.e.:

```c#
HandleIdentifier(SlimQueryNode identifier);
HandleIntConstant(SlimQueryNode intConst);
HandleOrNode(SlimQueryNode orNode);
// ...
```

This provides a visitor-like interface for `SlimQueryNode`. To test this out, I created an `ISyntacticTreeVisitor` that traverses OData's `QueryNode`
expression tree and rewrites the URL as a string, and I've done the same with the `SlimQueryNode` using a custom `ISyntacticTreeHandler` implementation.

They round trip results looks like:

- input: `"category eq 'electronics' or price gt 100"`
- output: `"((category eq 'electronics') or (price gt 100))"`

```md
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
```

| Method                                   | Mean         | Error      | StdDev       | Gen0   | Allocated |
|----------------------------------------- |-------------:|-----------:|-------------:|-------:|----------:|
| ParseExpression_ODataQueryOptionParser   | 23,070.22 ns | 493.339 ns | 1,423.396 ns | 1.3733 |    6024 B |
| ParseExpression_UriQueryExpressionParser |  1,686.49 ns |  33.224 ns |    39.550 ns | 0.2918 |    1264 B |
| ParseExpression_SlimQueryParser          |    188.74 ns |   3.598 ns |     3.190 ns | 0.0908 |     392 B |
| ParseExpression_SlimExpressionLexer      |     80.84 ns |   1.522 ns |     2.932 ns |      - |         - |
| QueryRoundTrip_UriQueryExpressionParser  |  1,835.62 ns |  12.187 ns |    10.804 ns | 0.4044 |    1752 B |
| QueryRoundTrip_SlimQueryParser           |    457.58 ns |   9.011 ns |    11.397 ns | 0.2036 |     880 B |

The results show that the lightweight parser is still much more performant. But I don't like the increase
in runtime and memory usage from the `SlimExpressionLexer` to the `SlimQueryParser` and its round-trip results.

I want to spend sometime to optimize the `SlimQueryParser` to reduce the overhead compared to the lexer so that
we have enough headroom when we add semantic binding and also when implement the full feature set.

- Consider using `byte[]` array instead of `ExpressionNode[]` array marshal data in and out of the array to see whether this reduces overhead
  - Use different structs for different kinds of nodes. E.g. unary nodes don't need a left and right node, we can skip those fields and save 8 bytes per unary node.
- Consider embedding the node class (e.g. binary operator, unary, etc.) in the enum values.
  - E.g. the least-significant word could be the operand "index" and the ms-word could have bit flags that represent whether or not the operator is a binary operator.

The `SlimQueryParser` implementation uses a simple precedence climbing technique to ensure the expression tree follows correct operator precedence.
Each operator is given a precedence value, which is used to decide to how to connect operator experessions. This works differently from OData's
recursive descent approach. I would like to compare the two techniques at some point to see which one is more efficient.

## Checkpoint 3 : Lightweight semantic parser

[**View checkpoint's commit: `d878b37`**](https://github.com/habbes/experiments/pull/3/commits/d878b3731e9cfa8a5581d99d400db86b09444fdd)

(with IEdmModel-based semantic context and validation)

This is a temporary checkpoint that demonstrates semantic binding. The `SemanticBinder` traverses the `SlimQueryNode` tree from the previous checkpoint (using the handler)
and creates a new `SemanticNode` that binds the syntactic node with semantic context (e.g. an identifier node is transformed into a property access node after verifying
that the property exists on the target type).

| Method                                   | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------------------------------- |-------------:|-----------:|-----------:|-------:|----------:|
| ParseExpression_ODataQueryOptionParser   | 21,583.65 ns | 411.359 ns | 440.150 ns | 1.3733 |    6024 B |
| ParseExpression_UriQueryExpressionParser |  1,685.28 ns |  27.102 ns |  25.351 ns | 0.2918 |    1264 B |
| ParseExpression_SlimSemanticBinder       |    578.86 ns |  10.906 ns |  10.711 ns | 0.2241 |     968 B |
| ParseExpression_SlimQueryParser          |    197.15 ns |   3.853 ns |   4.123 ns | 0.0908 |     392 B |
| ParseExpression_SlimExpressionLexer      |     82.37 ns |   1.504 ns |   1.334 ns |      - |         - |
| QueryRoundTrip_UriQueryExpressionParser  |  1,969.33 ns |  38.551 ns |  54.043 ns | 0.4044 |    1752 B |
| QueryRoundTrip_SlimQueryParser           |    470.51 ns |   9.110 ns |  12.470 ns | 0.2036 |     880 B |

I want to introduce a "lighter weight" semantic binder between the syntactic parser and this binder, one that does not allocate expression tree nodes, but indexes into syntatic nodes
and binds semantic context on demand (lazily). This could be ideal for a lightweight single-pass traversal of the tree. The semantic context could be cached if multiple passes are expected,
or if we expect multiple passes, we create an expression tree with all the nodes materialized, like what we have here.


## Checkpoint 4 : Support complex-nested expressions

TODO:

- Add support for in operator and array expressions
- Add support for parenthesized nested expressions
- optional: support for arithmetic operators
- optional: support for other data types
