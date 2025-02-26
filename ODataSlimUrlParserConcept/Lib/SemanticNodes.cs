using Microsoft.OData.Edm;

namespace Lib;

abstract public class SemanticNode
{
    public abstract SemanticNodeKind Kind { get;  }
    public abstract IEdmTypeReference EdmType { get; }
}

public enum SemanticNodeKind
{
    None,
    SingleValuePropertyAccess,
    OpenPropertyAccess,
    IntConstant,
    BoolConstant,
    StringConstant,
    Array,
    Or,
    And,
    Gt,
    Eq,
    In
}

public class SingleValuePropertyAccessNode : SemanticNode
{
    public SingleValuePropertyAccessNode(IEdmProperty property)
    {
        Property = property;
    }

    public IEdmProperty Property { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.SingleValuePropertyAccess;

    public override IEdmTypeReference EdmType => Property.Type;
}

public abstract class BinaryLogicalOperatorNode : SemanticNode
{
    public BinaryLogicalOperatorNode(SemanticNode left, SemanticNode right)
    {
        Left = left;
        Right = right;
    }
    public SemanticNode Left { get; private set; }
    public SemanticNode Right { get; private set; }

    public override IEdmTypeReference EdmType => EdmCoreModel.Instance.GetBoolean(isNullable: false);
}


public class OrNode : BinaryLogicalOperatorNode
{
    public OrNode(SemanticNode left, SemanticNode right) : base(left, right)
    {
    }

    public override SemanticNodeKind Kind => SemanticNodeKind.Or;
}

public class AndNode : BinaryLogicalOperatorNode
{
    public AndNode(SemanticNode left, SemanticNode right) : base(left, right)
    {
    }

    public override SemanticNodeKind Kind => SemanticNodeKind.And;
}

public class EqNode : BinaryLogicalOperatorNode
{
    public EqNode(SemanticNode left, SemanticNode right) : base(left, right)
    {
    }

    public override SemanticNodeKind Kind => SemanticNodeKind.Eq;
}

public class GtNode : BinaryLogicalOperatorNode
{
    public GtNode(SemanticNode left, SemanticNode right) : base(left, right)
    {
    }

    public override SemanticNodeKind Kind => SemanticNodeKind.Gt;
}

public class InNode : BinaryLogicalOperatorNode
{
    public InNode(SemanticNode left, SemanticNode right) : base(left, right)
    {
    }

    public override SemanticNodeKind Kind => SemanticNodeKind.In;
}

public class StringLiteralNode : SemanticNode
{
    public StringLiteralNode(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.StringConstant;
    public override IEdmTypeReference EdmType => EdmCoreModel.Instance.GetString(isNullable: false);
}

public class IntLiteralNode : SemanticNode
{
    public IntLiteralNode(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.IntConstant;
    public override IEdmTypeReference EdmType => EdmCoreModel.Instance.GetInt32(isNullable: false);
}

public class BoolLiteralNode : SemanticNode
{
    public BoolLiteralNode(bool value)
    {
        Value = value;
    }

    public bool Value { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.BoolConstant;
    public override IEdmTypeReference EdmType => EdmCoreModel.Instance.GetBoolean(isNullable: false);
}


public class OpenPropertyAccessNode : SemanticNode
{
    public OpenPropertyAccessNode(string propertyName)
    {
        PropertyName = propertyName;
    }
    public string PropertyName { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.OpenPropertyAccess;
    public override IEdmTypeReference EdmType => EdmCoreModel.Instance.GetUntyped();
}

public class ArrayNode : SemanticNode
{
    public ArrayNode(IEnumerable<SemanticNode> values)
    {
        Values = values;
    }

    public IEnumerable<SemanticNode> Values { get; private set; }

    public override SemanticNodeKind Kind => SemanticNodeKind.Array;
    public override IEdmTypeReference EdmType => EdmCoreModel.GetCollection(Values.First().EdmType);
}