namespace Lib;

public enum ExpressionNodeKind
{
    None = 0,
    Identifier,
    IntConstant,
    True,
    False,
    StringContant,
    Eq,
    Gt,
    And,
    Or,
    In,
    Array
}