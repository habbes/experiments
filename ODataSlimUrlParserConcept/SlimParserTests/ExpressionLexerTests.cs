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
        Assert.Equal("category", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("eq", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("electronics", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("or", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("price", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("gt", lexer.CurrentToken.Value.ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("100", lexer.CurrentToken.Value.ToString());

        Assert.False(lexer.Read());
    }
}