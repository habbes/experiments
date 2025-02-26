namespace Lib;

public partial class ExpressionParser : IDisposable
{
    readonly ReadOnlyMemory<char> _source;
    NodeList _nodes;
    bool _currentTokenPending = false;

    private static readonly int[] OperatorPrecedence = CreateOperatorPrecedenceMap();

    private static int[] CreateOperatorPrecedenceMap()
    {
        
        int[] map = new int[Enum.GetValues<ExpressionNodeKind>().Length];

        map[(int)ExpressionNodeKind.Or] = 1;
        map[(int)ExpressionNodeKind.And] = 2;
        map[(int)ExpressionNodeKind.Eq] = 3;
        map[(int)ExpressionNodeKind.Gt] = 3;
        map[(int)ExpressionNodeKind.In] = 3;

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

    //// This version does not take precedence into account
    //private int ParseExpression(ref ExpressionLexer lexer, ref NodeList nodes)
    //{
    //    lexer.Read();
    //    int left = ParseTerm(ref lexer, ref nodes);
    //    while (lexer.Read())
    //    {
    //        if (TryGetOperator(lexer.CurrentToken, out var op))
    //        {
    //            //lexer.Read();
    //            int right = ParseExpression(ref lexer, ref nodes);
    //            left = AddNode(ref nodes, new ExpressionNode(op, lexer.CurrentToken.Range, left, right));
    //        }
    //        else
    //        {
    //            // We don't expect consecutive terms without an operator
    //            throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
    //        }
    //    }

    //    return left;
    //}

    private int ParseExpressionWithPrecedence(ref ExpressionLexer lexer, ref NodeList nodes, int minPrecedence)
    {
        if (!(_currentTokenPending && lexer.IsInArray()))
        {
            lexer.Read();
        }

        _currentTokenPending = false;

        int left = ParseTerm(ref lexer, ref nodes);
        // this assumes the next token is an operator because we usually don't
        // expect consecutive terms.
        // But we can have consecutive terms in some cases:
        // - array values,
        // - function params
        // - select items, etc.
        // If the next item is an operator, then try to parse the operator expressation,
        // but it's a term then let's read it as a standalone valie in the next token
        lexer.Read();
        
        while (TryGetOperator(lexer.CurrentToken, out var op) &&
                TryGetOperatorPrecedence(op, out var precedence) &&
                precedence >= minPrecedence)
        {
            int right = ParseExpressionWithPrecedence(ref lexer, ref nodes, precedence + 1);
            left = AddNode(ref nodes, new ExpressionNode(op, lexer.CurrentToken.Range, left, right));
        }

        if (!nodes[left].Kind.IsBinaryOperator())
        {
            // we did not read an operator. This means
            // we have not process the current token that
            // we last read. We set this flag so that
            // we do not skip over it in the next parse.
            _currentTokenPending = true;
        }

        return left;
    }

    private int ParseTerm(ref ExpressionLexer lexer, ref NodeList nodes)
    {
        if (lexer.CurrentToken.Kind == ExpressionTokenKind.Identifier)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.Identifier, lexer.CurrentToken.Range));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.IntLiteral)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.IntConstant, lexer.CurrentToken.Range));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.StringLiteral)
        {
            return AddNode(ref nodes,new ExpressionNode(ExpressionNodeKind.StringContant, lexer.CurrentToken.Range));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.TrueLiteral)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.True, lexer.CurrentToken.Range));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.FalseLiteral)
        {
            return AddNode(ref nodes, new ExpressionNode(ExpressionNodeKind.False, lexer.CurrentToken.Range));
        }
        else if (lexer.CurrentToken.Kind == ExpressionTokenKind.OpenBracket)
        {
            return ParseArray(ref lexer, ref nodes);
        }
        else
        {
            throw new Exception($"Unexpected token {lexer.CurrentToken.Kind} {lexer.CurrentToken.Range.GetSpan(_source.Span)}");
        }
    }

    private int ParseArray(ref ExpressionLexer lexer, ref NodeList nodes)
    {
        int index = AddNode(ref nodes, new(ExpressionNodeKind.Array, lexer.CurrentToken.Range));

        //if (!lexer.Read())
        //{
        //    throw new Exception("Reached unexpected end of input while parsing array.");
        //}

        int firstChild = -1;
        int lastChild = -1;

        if (lexer.CurrentToken.Kind != ExpressionTokenKind.CloseBracket)
        {
            firstChild = lastChild = ParseExpressionWithPrecedence(ref lexer, ref nodes, 0);
        }
        
        //if (!lexer.Read())
        //{
        //    throw new Exception("Reached unexpected end of input while parsing array.");
        //}

        while (lexer.CurrentToken.Kind != ExpressionTokenKind.CloseBracket)
        {
            lastChild = ParseExpressionWithPrecedence(ref lexer, ref nodes, 0);
        }

        // last token was CloseBracket, read to consume the token
        lexer.Read();

        ref var arrayNode = ref nodes[index];
        arrayNode.FirstChild = firstChild;
        arrayNode.LastChild = lastChild;

        return index;
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
        else if (tokenValue.Equals("in", StringComparison.Ordinal))
        {
            op = ExpressionNodeKind.In;
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
}
