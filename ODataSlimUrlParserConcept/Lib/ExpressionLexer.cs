namespace Lib;

public ref struct ExpressionLexer
{
    ReadOnlySpan<char> _source;
    Token _token;
    int _pos = 0;

    public ExpressionLexer(ReadOnlySpan<char> source)
    {
        _source = source;
    }

    public Token CurrentToken => _token;

    public bool Read()
    {
        if (!this.SkipOverWhitespace())
        {
            return false;
        }

        if (_source.Length == _pos)
        {
            return false;
        }

        if (_source[_pos] == '\'')
        {
            this.ReadString();
            return true;
        }

        if (IsDigit(_source[_pos]))
        {
            this.ReadInteger();
            return true;
        }

        if (IsAlpha(_source[_pos]))
        {
            this.ReadIdentifier();
            return true;
        }

        throw new Exception($"Unexpected token at {_source.ToString()}");
    }

    private bool SkipOverWhitespace()
    {
        while (_pos < _source.Length && _source[_pos] == ' ')
        {
           _pos++;
        }
       
        if (_pos == _source.Length)
        {
            return false;
        }

        return true;
    }

    private void ReadString()
    {
        int start = ++_pos; // skip opening quote
        // Naive string, assume no escaping
        while (_pos < _source.Length && _source[_pos] != '\'')
        {
            _pos++;
        }

        if (_pos == _source.Length)
        {
            throw new Exception("Reached unexpected end of input");
        }

        _token = new Token()
        {
            Kind = ExpressionTokenKind.StringLiteral,
            Range = new ValueRange(start, _pos - start)
        };

        _pos++; // slip closing quote
    }

    private void ReadInteger()
    {
        int start = _pos;
        while (_pos < _source.Length && IsDigit(_source[_pos]))
        {
            _pos++;
        }


        _token = new Token()
        {
            Kind = ExpressionTokenKind.IntLiteral,
            Range = new ValueRange(start, _pos - start)
        };
    }

    private void ReadIdentifier()
    {
        int start = _pos;
        _pos++; // the caller ensures that the first char is an alpha char

        while (_pos < _source.Length && (IsAlpha(_source[_pos]) || IsDigit(_source[_pos])))
        {
            _pos++;
        }

        _token = new Token()
        {
            Kind = ExpressionTokenKind.Identifier,
            Range = new ValueRange(start, _pos - start)
        };
    }

    private static bool IsDigit(char c) => c >= '0' && c <= '9';
    private static bool IsAlpha(char c) => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    
    public struct Token
    {
        public ValueRange Range { get; internal set; }
        public ExpressionTokenKind Kind { get; internal set; }
    }
}
