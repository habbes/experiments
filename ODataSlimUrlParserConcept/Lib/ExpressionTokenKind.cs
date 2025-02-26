namespace Lib;

public enum ExpressionTokenKind
{
    None,
    // For simplicity, we use the identifier token for operators and keywords
    // as well, and disambiguate them in the parser.
    Identifier,
    StringLiteral,
    IntLiteral,
    TrueLiteral,
    FalseLiteral,
    /// <summary>
    /// The token '[' used to open an array.
    /// </summary>
    OpenBracket,
    /// <summary>
    /// The token ']' used to close an array.
    /// </summary>
    CloseBracket,
}
