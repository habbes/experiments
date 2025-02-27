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
        // We place commas and brackets to ensure they are correctly parsed as parts of strings and not as delimiters.
        ReadOnlySpan<char> source = "category in ('electronics,,tech', 'books()', name, 1, false)";
        ExpressionLexer lexer = new ExpressionLexer(source);

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("category", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.Identifier, lexer.CurrentToken.Kind);
        Assert.Equal("in", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenParen, lexer.CurrentToken.Kind);
        Assert.Equal("(", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("electronics,,tech", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.StringLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("books()", lexer.CurrentToken.Range.GetSpan(source).ToString());

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
        Assert.Equal(ExpressionTokenKind.CloseParen, lexer.CurrentToken.Kind);
        Assert.Equal(")", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.False(lexer.Read());
    }

    [Fact]
    public void ParseNestedArrayExpressionAndEmitCorrectTokens()
    {
        ReadOnlySpan<char> source = "((12, 3), 4, ( 5, (6, 8)))";
        ExpressionLexer lexer = new ExpressionLexer(source);

        // write test
        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenParen, lexer.CurrentToken.Kind);
        Assert.Equal("(", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenParen, lexer.CurrentToken.Kind);
        Assert.Equal("(", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("12", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("3", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.CloseParen, lexer.CurrentToken.Kind);
        Assert.Equal(")", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("4", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenParen, lexer.CurrentToken.Kind);
        Assert.Equal("(", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("5", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.OpenParen, lexer.CurrentToken.Kind);
        Assert.Equal("(", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("6", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.IntLiteral, lexer.CurrentToken.Kind);
        Assert.Equal("8", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.CloseParen, lexer.CurrentToken.Kind);
        Assert.Equal(")", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.CloseParen, lexer.CurrentToken.Kind);
        Assert.Equal(")", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.True(lexer.Read());
        Assert.Equal(ExpressionTokenKind.CloseParen, lexer.CurrentToken.Kind);
        Assert.Equal(")", lexer.CurrentToken.Range.GetSpan(source).ToString());

        Assert.False(lexer.Read());
    }

    [Theory]
    [InlineData("(")]
    [InlineData("((12, 3), 4, ( 5, (6, 8))")]
    public void ThrowsExceptionWhenArrayNotTerminated(string source)
    {
        Exception error = Assert.Throws<Exception>(() =>
        {
            var lexer = new ExpressionLexer(source);
            while (lexer.Read()) { }
        });

        Assert.Equal("Expected ')' but reached end of input.", error.Message);
    }

    [Theory]
    [InlineData(")", 0)]
    [InlineData("((12, 3), 4, ( 5, (6, 8))))", 26)]
    public void ThrowExceptionWhenArrayClosingBracketIsNotMatched(string source, int errorPosition)
    {
        Exception error = Assert.Throws<Exception>(() =>
        {
            var lexer = new ExpressionLexer(source);
            while (lexer.Read()) { }
        });

        Assert.Equal($"Unexpected ')' at position {errorPosition}.", error.Message);
    }

    [Theory]
    [InlineData("(,", 1)]
    [InlineData("(12, ,4)", 5)]
    public void ThrowsExceptionWhenArrayContainsMisplacedCommas(string source, int errorPosition)
    {
        Exception error = Assert.Throws<Exception>(() =>
        {
            var lexer = new ExpressionLexer(source);
            while (lexer.Read()) { }
        });

        Assert.StartsWith($"Unexpected ',' at position {errorPosition}", error.Message);
    }

    [Fact]
    public void ThrowsExceptionWhenArrayClosedAfterComma()
    {
        string source = "(12, 4,)";
        Exception error = Assert.Throws<Exception>(() =>
        {
            var lexer = new ExpressionLexer(source);
            while (lexer.Read()) { }
        });

        Assert.StartsWith($"Unexpected ')' at position 7", error.Message);
    }
}