namespace Lib;

public struct SlimQueryNode
{
    private ExpressionParser _parent;
    private int _index;

    internal SlimQueryNode(ExpressionParser parent, int index)
    {
        _parent = parent;
        _index = index;
    }

    public ExpressionNodeKind Kind => _parent[_index].Kind;

    public SlimQueryNode GetLeft()
    {
        if (!this.Kind.IsBinaryOperator())
        {
            throw new InvalidOperationException("Node is not a binary operator");
        }

        return new SlimQueryNode(_parent, _parent[_index].Left);
    }

    public SlimQueryNode GetRight()
    {
        if (!this.Kind.IsBinaryOperator())
        {
            throw new InvalidOperationException("Node is not a binary operator");
        }

        return new SlimQueryNode(_parent, _parent[_index].Right);
    }

    public ReadOnlySpan<char> GetRawValueSpan()
    {
        return _parent.GetValueSpan(_index);
    }

    public ReadOnlyMemory<char> GetRawValueMemory()
    {
        return _parent.GetValueMemory(_index);
    }

    public int GetInt()
    {
        if (this.Kind != ExpressionNodeKind.IntConstant)
        {
            throw new InvalidOperationException("Node is not an integer constant");
        }
        return int.Parse(GetRawValueSpan());
    }

    public string GetString()
    {
        if (this.Kind != ExpressionNodeKind.StringContant)
        {
            throw new InvalidOperationException("Node is not a string constant");
        }
        return GetRawValueSpan().ToString();
    }

    public string GetIdentifier()
    {
        if (this.Kind != ExpressionNodeKind.Identifier)
        {
            throw new InvalidOperationException("Node is not an identifier");
        }
        return GetRawValueSpan().ToString();
    }
}
