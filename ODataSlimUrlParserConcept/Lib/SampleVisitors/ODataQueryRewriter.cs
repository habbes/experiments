using System.Text;
using Microsoft.OData.UriParser.Aggregation;
using Microsoft.OData.UriParser;

namespace Lib.SampleVisitors;

public class ODataQueryRewriter : ISyntacticTreeVisitor<bool>
{
    StringBuilder query = new();

    public string GetQuery()
    {
        return query.ToString();
    }

    public bool Visit(AllToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(AnyToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(BinaryOperatorToken tokenIn)
    {
        query.Append('(');
        tokenIn.Left.Accept(this);
        query.Append(' ');
        switch (tokenIn.OperatorKind)
        {
            case BinaryOperatorKind.And:
                query.Append("and");
                break;
            case BinaryOperatorKind.Or:
                query.Append("or");
                break;
            case BinaryOperatorKind.GreaterThan:
                query.Append("gt");
                break;
            case BinaryOperatorKind.Equal:
                query.Append("eq");
                break;
            default:
                throw new NotImplementedException();
        }

        query.Append(' ');
        tokenIn.Right.Accept(this);
        query.Append(')');

        return true;
    }

    public bool Visit(CountSegmentToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(InToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(DottedIdentifierToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(ExpandToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(ExpandTermToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(FunctionCallToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(LambdaToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(LiteralToken tokenIn)
    {
        if (tokenIn.Value is string)
        {
            query.Append('\'');
            query.Append(tokenIn.Value);
            query.Append('\'');
        }
        else
        {
            query.Append(tokenIn.Value);
        }

        return true;
    }

    public bool Visit(InnerPathToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(OrderByToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(EndPathToken tokenIn)
    {
        query.Append(tokenIn.Identifier);
        return true;
    }

    public bool Visit(CustomQueryOptionToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(RangeVariableToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(SelectToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(SelectTermToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(StarToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(UnaryOperatorToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(FunctionParameterToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(AggregateToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(AggregateExpressionToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(EntitySetAggregateToken tokenIn)
    {
        throw new NotImplementedException();
    }

    public bool Visit(GroupByToken tokenIn)
    {
        throw new NotImplementedException();
    }
}