using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public class ExpressionParser
{
    readonly ReadOnlyMemory<char> _source;
    readonly List<ExpressionNode> _nodes = []; // TODO: consider lazy initialization

    private ExpressionParser(ReadOnlyMemory<char> source)
    {
        _source = source;
    }

    private SlimQueryNode Parse()
    {
        var lexer = new ExpressionLexer(_source.Span);

        int root = ParseExpression(ref lexer);
        return new SlimQueryNode(this, root);
    }

    public static SlimQueryNode Parse(ReadOnlyMemory<char> source)
    {
        return new ExpressionParser(source).Parse();
    }

    internal ExpressionNode this[int index] => _nodes[index];

    internal ReadOnlySpan<char> GetValueSpan(int index) => _nodes[index].Range.GetSpan(_source.Span);

    internal ReadOnlyMemory<char> GetValueMemory(int index) => _nodes[index].Range.GetMemory(_source);

    private int ParseExpression(ref ExpressionLexer lexer)
    {
        lexer.Read();
        int left = ParseTerm(ref lexer);
        while (lexer.Read())
        {
            if (TryGetOperator(lexer.CurrentToken, out var op))
            {
                //lexer.Read();
                int right = ParseExpression(ref lexer);
                left = AddNode(new ExpressionNode(op, lexer.CurrentToken.Range, left, right));
            }
            else
            {
                // We don't expect consecutive terms without an operator
                throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
            }
        }

        return left;
    }

    private int ParseTerm(ref ExpressionLexer lexer)
    {
        if (lexer.CurrentToken.Kind == ExpressionTokenKind.Identifier)
        {
            return AddNode(new ExpressionNode(ExpressionNodeKind.Identifier, lexer.CurrentToken.Range, 0, 0));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.IntLiteral)
        {
            return AddNode(new ExpressionNode(ExpressionNodeKind.IntConstant, lexer.CurrentToken.Range, 0, 0));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.StringLiteral)
        {
            return AddNode(new ExpressionNode(ExpressionNodeKind.StringContant, lexer.CurrentToken.Range, 0, 0));
        }
        else
        {
            throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
        }

    }

    private int AddNode(ExpressionNode node)
    {
        _nodes.Add(node);
        return _nodes.Count - 1;
    }

    private bool TryGetOperator(ExpressionLexer.Token token, out ExpressionNodeKind op)
    {
        op = default;
        if (token.Kind != ExpressionTokenKind.Identifier)
        {
            return false;
        }

        ReadOnlySpan<char> tokenValue = token.Range.GetSpan(_source.Span);
        if (tokenValue.Equals("eq", StringComparison.Ordinal))
        {
            op = ExpressionNodeKind.Eq;
            return true;
        }
        else if (tokenValue.Equals("gt", StringComparison.Ordinal))
        {
            op = ExpressionNodeKind.Gt;
            return true;
        }
        else if (tokenValue.Equals("and", StringComparison.Ordinal))
        {
            op = ExpressionNodeKind.And;
            return true;
        }
        else if (tokenValue.Equals("or", StringComparison.Ordinal))
        {
            op = ExpressionNodeKind.Or;
            return true;
        }
        else
        {
            return false;
        }
    }

    internal record struct ExpressionNode(ExpressionNodeKind Kind, ValueRange Range, int Left, int Right);
}
