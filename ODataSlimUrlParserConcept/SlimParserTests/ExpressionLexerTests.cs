using Lib;

namespace SlimParserTests;

public class ExpressionLexerTests
{
    [Fact]
    public void ParsersExpressionSourceAndEmitsCorrectTokens()
    {
        ReadOnlySpan<char> source = "category eq 'electronics' or price gt 100";
        ExpressionLexer lexer = new ExpressionLexer(source);

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("category", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("eq", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("electronics", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("or", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("price", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("gt", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("100", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.False(lexer.Read());
    }

    [Fact]
    public void ParsesArrayExpressionAndEmitsCorrectTokens()
    {
        ReadOnlySpan<char> source = "category in ['electronics', 'books', name, 1, false]";
        ExpressionLexer lexer = new ExpressionLexer(source);

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("category", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("in", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenBracket, lexer.CurrentToken.Kind);
        Assert.Equal("[", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("electronics", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("books", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("name", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("1", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.FalseLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("false", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.CloseBracket, lexer.CurrentToken.Kind);
        Assert.Equal("]", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.False(lexer.Read());
    }
}