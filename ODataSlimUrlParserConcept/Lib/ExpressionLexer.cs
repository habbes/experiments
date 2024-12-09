namespace Lib;

public ref struct ExpressionLexer
{
    ReadOnlySpan<char> _source;
    Token _token;
    bool _isEnd = false;

    public ExpressionLexer(ReadOnlySpan<char> source)
    {
        _source = source;
    }

    public Token CurrentToken => _token;

    public bool Read()
    {
        if (_isEnd)
        {
            return false;
        }

        if (!this.SkipOverWhitespace())
        {
            return false;
        }

        if (_source.Length == 0)
        {
            return false;
        }

        if (_source[0] == '\'')
        {
            this.ReadString();
            return true;
        }

        if (IsDigit(_source[0]))
        {
            this.ReadInteger();
            return true;
        }

        if (IsAlpha(_source[0]))
        {
            this.ReadIdentifier();
            return true;
        }

        throw new Exception($"Unexpected token at {_source.ToString()}");
    }

    private bool SkipOverWhitespace()
    {
        int i = 0;
        while (i < _source.Length && _source[i] == ' ')
        {
            i++;
        }
       
        if (i == _source.Length)
        {
            return false;
        }

        _source = _source[i..];
        return true;
    }

    private void ReadString()
    {
        int start = 1;
        int i = start; // skip opening quote
        // Naive string, assume no escaping
        while (i < _source.Length && _source[i] != '\'')
        {
            i++;
        }

        if (i == _source.Length)
        {
            throw new Exception("Reached unexpected end of input");
        }

        _token = new Token()
        {
            Kind = ExpressionTokenKind.StringLiteral,
            Value = _source[start..i]
        };

        int next = i + 1; // slip closing quote
        TryAdvanceSource(next);
    }

    private void ReadInteger()
    {
        int start = 0;
        int i = start;
        while (i < _source.Length && IsDigit(_source[i]))
        {
            i++;
        }


        _token = new Token()
        {
            Kind = ExpressionTokenKind.IntLiteral,
            Value = _source[start..i]
        };

        TryAdvanceSource(i);
    }

    private void ReadIdentifier()
    {
        int start = 0;
        int i = start + 1; // the caller ensures that the first char is an alpha char

        while (i < _source.Length && (IsAlpha(_source[i]) || IsDigit(_source[i])))
        {
            i++;
        }

        _token = new Token()
        {
            Kind = ExpressionTokenKind.Identifier,
            Value = _source[start..i]
        };

        TryAdvanceSource(i);
    }

    private void TryAdvanceSource(int next)
    {
        if (next == _source.Length)
        {
            _isEnd = true;
            return;
        }

        _source = _source[next..];
    }

    private static bool IsDigit(char c) => c >= '0' && c <= '9';
    private static bool IsAlpha(char c) => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    
    public ref struct Token
    {
        public ReadOnlySpan<char> Value { get; internal set; }
        public ExpressionTokenKind Kind { get; internal set; }
    }
}
