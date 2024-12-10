namespace Lib;

public partial class ExpressionParser : IDisposable
{
    readonly ReadOnlyMemory<char> _source;
    NodeList _nodes;

    private static readonly int[] OperatorPrecedence = CreateOperatorPrecedenceMap();

    private static int[] CreateOperatorPrecedenceMap()
    {
        
        int[] map = new int[Enum.GetValues<ExpressionNodeKind>().Length];

        map[(int)ExpressionNodeKind.Or] = 1;
        map[(int)ExpressionNodeKind.And] = 2;
        map[(int)ExpressionNodeKind.Eq] = 3;
        map[(int)ExpressionNodeKind.Gt] = 3;

        return map;
    }

    private ExpressionParser(ReadOnlyMemory<char> source)
    {
        _source = source;
    }

    public static SlimQueryNode Parse(ReadOnlyMemory<char> source)
    {
        var nodes = new NodeList(4);
        var parser = new ExpressionParser(source);
        var root = parser.Parse(ref nodes);
        parser._nodes = nodes;

        return root;
    }

    private SlimQueryNode Parse(ref NodeList nodes)
    {
        var lexer = new ExpressionLexer(_source.Span);
        
        //int root = ParseExpression(ref lexer);
        int root = ParseExpressionWithPrecedence(ref lexer, ref nodes, 0);
        return new SlimQueryNode(this, root);
    }

    internal ExpressionNode this[int index] => _nodes[index];

    internal ReadOnlySpan<char> GetValueSpan(int index) => _nodes[index].Range.GetSpan(_source.Span);

    internal ReadOnlyMemory<char> GetValueMemory(int index) => _nodes[index].Range.GetMemory(_source);

    // This version does not take precedence into account
    private int ParseExpression(ref ExpressionLexer lexer, ref NodeList nodes)
    {
        lexer.Read();
        int left = ParseTerm(ref lexer, ref nodes);
        while (lexer.Read())
        {
            if (TryGetOperator(lexer.CurrentToken, out var op))
            {
                //lexer.Read();
                int right = ParseExpression(ref lexer, ref nodes);
                left = AddNode(ref nodes, new ExpressionNode(op, lexer.CurrentToken.Range, left, right));
            }
            else
            {
                // We don't expect consecutive terms without an operator
                throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
            }
        }

        return left;
    }

    private int ParseExpressionWithPrecedence(ref ExpressionLexer lexer, ref NodeList nodes, int minPrecedence)
    {
        lexer.Read();
        int left = ParseTerm(ref lexer, ref nodes);

        lexer.Read();
        while (TryGetOperator(lexer.CurrentToken, out var op) &&
                TryGetOperatorPrecedence(op, out var precedence) &&
                precedence >= minPrecedence)
        {
            int right = ParseExpressionWithPrecedence(ref lexer, ref nodes, precedence + 1);
            left = AddNode(ref nodes, new ExpressionNode(op, lexer.CurrentToken.Range, left, right));
        }

        return left;
    }

    private int ParseTerm(ref ExpressionLexer lexer, ref NodeList nodes)
    {
        if (lexer.CurrentToken.Kind == ExpressionTokenKind.Identifier)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.Identifier, lexer.CurrentToken.Range, 0, 0));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.IntLiteral)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.IntConstant, lexer.CurrentToken.Range, 0, 0));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.StringLiteral)
        {
            return AddNode(ref nodes,new ExpressionNode(ExpressionNodeKind.StringContant, lexer.CurrentToken.Range, 0, 0));
        }
        else
        {
            throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
        }

    }

    private static int AddNode(ref NodeList nodes, ExpressionNode node)
    {
        nodes.Add(node);
        return nodes.Count - 1;
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

    private static bool TryGetOperatorPrecedence(ExpressionNodeKind op, out int precedence)
    {
        precedence = OperatorPrecedence[(int)op];
        return precedence > 0;
    }

    public void Dispose()
    {
        _nodes.Dispose();
    }

    internal record struct ExpressionNode(ExpressionNodeKind Kind, ValueRange Range, int Left, int Right);
}
