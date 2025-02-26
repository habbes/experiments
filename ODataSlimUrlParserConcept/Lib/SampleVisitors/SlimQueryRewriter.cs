using System.Text;

namespace Lib.SampleVisitors;

public class SlimQueryRewriter : ISyntacticExpressionHandler<bool>
{
    StringBuilder query = new();

    public string GetQuery()
    {
        return query.ToString();
    }

    private void HandleBinaryOp(SlimQueryNode op, string opValue)
    {
        query.Append('(');
        op.GetLeft().Accept(this);
        query.Append(' ');

        query.Append(opValue);

        query.Append(' ');
        op.GetRight().Accept(this);
        query.Append(')');
    }

    public bool HandleAnd(SlimQueryNode andNode)
    {
        HandleBinaryOp(andNode, "and");

        return true;
    }

    public bool HandleEq(SlimQueryNode eqNode)
    {
        HandleBinaryOp(eqNode, "eq");
        return true;
    }

    public bool HandleGt(SlimQueryNode gtNode)
    {
        HandleBinaryOp(gtNode, "gt");
        return true;
    }

    public bool HandleIdentifier(SlimQueryNode identifier)
    {
        query.Append(identifier.GetRawValueSpan());
        return true;
    }

    public bool HandleIntConstant(SlimQueryNode intConst)
    {
        query.Append(intConst.GetRawValueSpan());
        return true;
    }

    public bool HandleOr(SlimQueryNode orNode)
    {
        HandleBinaryOp(orNode, "or");
        return true;
    }

    public bool HandleIn(SlimQueryNode inNode)
    {
        HandleBinaryOp(inNode, "in");
        return true;
    }

    public bool HandleStringConstant(SlimQueryNode stringConst)
    {
        query.Append('\'');
        query.Append(stringConst.GetRawValueSpan());
        query.Append('\'');
        return true;
    }

    public bool HandleBoolConstant(SlimQueryNode boolConst)
    {
        query.Append(boolConst.Kind == ExpressionNodeKind.True ? "true" : "false");
        return true;
    }

    public bool HandleArray(SlimQueryNode arrayNode)
    {
        query.Append('[');

        var enumerator = arrayNode.EnumerateArray();
        if (enumerator.MoveNext())
        {
            enumerator.Current.Accept(this);
        }
      
        foreach (var item in enumerator)
        {
            query.Append(", ");
            item.Accept(this);
        }

        query.Append(']');

        return true;
    }
}