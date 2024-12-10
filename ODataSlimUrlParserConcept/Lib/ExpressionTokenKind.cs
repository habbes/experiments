namespace Lib;

public enum ExpressionTokenKind
{
    None,
    // For simplicity, we use the identifier token for operators and keywords
    // as well, and disambiguate them in the parser.
    Identifier,
    StringLiteral,
    IntLiteral,
}
