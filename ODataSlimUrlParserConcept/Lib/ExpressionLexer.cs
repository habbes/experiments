using System.Diagnostics;

namespace Lib;

[DebuggerDisplay("ExpressionLexer ({_token.Kind} @ {_pos} [{_token.Range.GetSpan(_source)}])")]
public ref struct ExpressionLexer
{
    ReadOnlySpan<char> _source;
    Token _token;
    int _pos = 0;
    int _arrayDepth = 0;

    public ExpressionLexer(ReadOnlySpan<char> source)
    {
        _source = source;
    }

    public Token CurrentToken => _token;

    public bool Read()
    {
        if (!this.SkipOverWhitespace())
        {
            if (_arrayDepth > 0)
            {
                throw new Exception("Expected ']' but reached end of input.");
            }

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

       
        if (_source[_pos] == '[')
        {
            // OData advances through the text until it finds a matching close ']'
            // I think that's inefficient as it leads to multiple passes through the text.
            // What I can do is either keep track of the open/close brackets in some state
            // field and update it whenever I see open/close bracket. Alternatively, I
            // can handle this in a higher-level parser. I'll evaluate the cost of handling
            // it here to decide which approach to take.
            // Also, at this stage OData only matches the brackets, doesn't check whether
            // nested ( or { are balanced. That check probably happens later.
            this.ReadArrayStart();
            return true;
        }

        if (_source[_pos] == ']')
        {
            this.ReadArrayEnd();
            return true;
        }

        if (_source[_pos] == ',')
        {
            

        }

        throw new Exception($"Unexpected token at {_source.Slice(_pos).ToString()}");
    }

    private bool SkipOverWhitespace()
    {
        // TODO: skipping over commas is a hack for now. We should detect
        // commas at invalid positions and handle that.
        while (_pos < _source.Length && (_source[_pos] == ' ' || _source[_pos] == ','))
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

        // TODO: this does another scan to check whether value is known keyword
        // It may be possible to optimize by checking the keyword as we scan
        // For example, before scan the entire identifier, if the first char is 't'
        // we can check whether the identifier is 'true' and if so, set the token, etc.
        var identifier = _source.Slice(start, _pos - start);
        if (identifier.Equals("true", StringComparison.Ordinal))
        {
            _token = new Token()
            {
                Kind = ExpressionTokenKind.TrueLiteral,
                Range = new ValueRange(start, _pos - start)
            };
            return;
        }

        if (identifier.Equals("false", StringComparison.Ordinal))
        {
            _token = new Token()
            {
                Kind = ExpressionTokenKind.FalseLiteral,
                Range = new ValueRange(start, _pos - start)
            };
            return;
        }

        _token = new Token()
        {
            Kind = ExpressionTokenKind.Identifier,
            Range = new ValueRange(start, _pos - start)
        };
    }

    private void ReadArrayStart()
    {
        // Caller ensures that the current char is '['
        _token = new Token()
        {
            Kind = ExpressionTokenKind.OpenBracket,
            Range = new ValueRange(_pos, 1)
        };

        _arrayDepth++;
        _pos++;
    }

    private void ReadArrayEnd()
    {
        // Caller ensures that the current char is ']'
        _token = new Token()
        {
            Kind = ExpressionTokenKind.CloseBracket,
            Range = new ValueRange(_pos, 1)
        };

        _arrayDepth--;
        _pos++;
    }

    private static bool IsDigit(char c) => c >= '0' && c <= '9';
    private static bool IsAlpha(char c) => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    
    public struct Token
    {
        public ValueRange Range { get; internal set; }
        public ExpressionTokenKind Kind { get; internal set; }
    }
}
