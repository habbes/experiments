using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public struct HybridListCharStack : ICharStack
{
    private CharBuffer _buffer;
    private List<char>? _overflow;
    private int _count;

    public void Push(char c)
    {
        if (_overflow != null)
        {
            _overflow.Add(c);
        }
        else if (_count < CharBuffer.Length)
        {
            _buffer[_count] = c;
        }
        else
        {
            // Initialize the overflow stack with a reasonable capacity
            // to avoid frequent resizing.
            _overflow = new List<char>(CharBuffer.Length) { c };
        }
        _count++;
    }

    public char Pop()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException();
        }


        if (_overflow != null && _count > CharBuffer.Length)
        {
            char value = _overflow[^1];
            _overflow.RemoveAt(_overflow.Count - 1);
            _count--;
            return value;
        }
        

        _count--;
        return _buffer[_count];
    }

    public readonly int Count => _count;
}

