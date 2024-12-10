namespace Lib;

public enum ExpressionNodeKind
{
    None = 0,
    Identifier,
    IntConstant,
    StringContant,
    Eq,
    Gt,
    And,
    Or,
}