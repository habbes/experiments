﻿using Lib;
using Lib.SampleVisitors;
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
    public void GeneratesCorrectQueryWithQueryRewriter()
    {
        string source = "category eq 'electronics' or price gt 100";
        SlimQueryNode node = ExpressionParser.Parse(source.AsMemory());
        SlimQueryRewriter rewriter = new();
        node.Accept(rewriter);
        Assert.Equal("((category eq 'electronics') or (price gt 100))", rewriter.GetQuery());
    }
}
