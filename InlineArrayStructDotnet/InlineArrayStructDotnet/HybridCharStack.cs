using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public struct HybridCharStack : ICharStack
{
    private CharBuffer _buffer;
    private Stack<char>? _overflow;
    private int _count;

    public void Push(char c)
    {
        if (_count < CharBuffer.Length)
            _buffer[_count] = c;
        else
        {
            // Initialize the overflow stack with a reasonable capacity
            // to avoid frequent resizing.
            _overflow ??= new Stack<char>(CharBuffer.Length);
            _overflow.Push(c);
        }
        _count++;
    }

    public char Pop()
    {
        if (_count == 0)
            throw new InvalidOperationException();

        _count--;

        if (_count < CharBuffer.Length)
            return _buffer[_count];
        else
        {
            var val = _overflow!.Pop();
            return val;
        }
    }

    public readonly int Count => _count;
}

