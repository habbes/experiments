using InlineArrayStructDotnet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public struct HybridArrayCharStack : ICharStack
{
    CharBuffer _buffer;
    char[]? _array;
    int _count;
    public readonly int Count => _count;

    public char Pop()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException();
        }

        _count--;
        if (_array != null)
        {
            return _array[_count];
        }

        return _buffer[_count];
    }

    public void Push(char c)
    {
        if (_array != null)
        {
            EnsureCapacity();
            _array[_count++] = c;
            return;
        }

        if (_count < CharBuffer.Length)
        {
            _buffer[_count++] = c;
            return;
        }
        
        _array = new char[CharBuffer.Length * 2];
        ((Span<char>)_buffer).CopyTo(_array);
        _array[_count++] = c;
    }

    private void EnsureCapacity()
    {
        Debug.Assert(_array != null);
        if (_count < _array.Length)
        {
            return;
        }

        Array.Resize(ref _array, _array.Length * 2);
    }
}
