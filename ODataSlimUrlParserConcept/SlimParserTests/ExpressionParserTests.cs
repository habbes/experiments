﻿using Lib;
using Lib.SampleVisitors;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimParserTests;

public class ExpressionParserTests
{
    [Fact]
    public void ParsersSourceAndGeneratesCorrectNodes()
    {
        string source = "category eq 'electronics' or price gt 100";
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());
        Assert.Equal(ExpressionNodeKind.Or, node.Kind);
        Assert.Equal(ExpressionNodeKind.Eq, node.GetLeft().Kind);
        Assert.Equal(ExpressionNodeKind.Gt, node.GetRight().Kind);
        Assert.Equal("category", node.GetLeft().GetLeft().GetIdentifier());
        Assert.Equal("electronics", node.GetLeft().GetRight().GetString());
        Assert.Equal("price", node.GetRight().GetLeft().GetIdentifier());
        Assert.Equal(100, node.GetRight().GetRight().GetInt());
    }

    [Fact]
    public void ParsesArrayExpressions()
    {
        string source = "category in ('electronics', 1, (2,3,true))";
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());
        Assert.Equal(ExpressionNodeKind.In, node.Kind);
        Assert.Equal("category", node.GetLeft().GetIdentifier());
        SlimQueryNode arrayNode = node.GetRight();
        Assert.Equal(ExpressionNodeKind.Array, arrayNode.Kind);
        Assert.Equal(3, arrayNode.GetCount());
        Assert.Equal("electronics", arrayNode.GetElement(0).GetString());
        Assert.Equal(1, arrayNode.GetElement(1).GetInt());

        var innerArrayNode = arrayNode.GetElement(2);
        Assert.Equal(ExpressionNodeKind.Array, innerArrayNode.Kind);
        Assert.Equal(3, innerArrayNode.GetCount());
        Assert.Equal(2, innerArrayNode.GetElement(0).GetInt());
        Assert.Equal(3, innerArrayNode.GetElement(1).GetInt());
        Assert.True(innerArrayNode.GetElement(2).GetBoolean());
    }

    [Theory]
    [InlineData(
        "category eq 'electronics' or price gt 100",
        "((category eq 'electronics') or (price gt 100))")]
    [InlineData(
        "category in ('electronics', 1, (2,3,true))",
        "(category in ('electronics', 1, (2, 3, true)))")]
    public void GeneratesCorrectQueryWithQueryRewriter(string source, string expected)
    {
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());
        SlimQueryRewriter rewriter = new();
        node.Accept(rewriter);
        Assert.Equal(expected, rewriter.GetQuery());
    }

    // Use the same test cases as in the previous test to ensure
    // OData and Slim parser generate the same results.
    [Theory]
    [InlineData(
        "category eq 'electronics' or price gt 100",
        "((category eq 'electronics') or (price gt 100))")]
    [InlineData(
        "category in ('electronics', 1, (2,3,true))",
        // OData parser parses the array as literal and stores it as a string.
        // It's only transformed into an collection later on by the semantic binder.
        "(category in '('electronics', 1, (2,3,true))')")]
    public void ODataQueryRewriterGeneratesCorrectQuery(string source, string expected)
    {
        var rewriter = new ODataQueryRewriter();
        UriQueryExpressionParser parser = new(100);

        QueryToken expression = parser.ParseFilter(source);
        expression.Accept(rewriter);

        Assert.Equal(expected, rewriter.GetQuery());
    }
}
