using InlineArrayStructDotnet;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public struct HybridPooledArrayCharStack : ICharStack
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
        
        _array = ArrayPool<char>.Shared.Rent(CharBuffer.Length * 2);
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

        var oldArray = _array;
        _array = ArrayPool<char>.Shared.Rent(_array.Length * 2);
        Array.Copy(oldArray, _array, oldArray.Length);
        ArrayPool<char>.Shared.Return(oldArray);
    }

    public void Dispose()
    {
        if (_array != null)
        {
            ArrayPool<char>.Shared.Return(_array);
            _array = null;
        }
    }
}
