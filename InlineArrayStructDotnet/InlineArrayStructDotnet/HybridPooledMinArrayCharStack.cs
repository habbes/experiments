using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public struct HybridPooledMinArrayCharStack : ICharStack
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
        if (_count < CharBuffer.Length)
        {
            return _buffer[_count];
        }

        return _array![_count - CharBuffer.Length];
    }

    public void Push(char c)
    {
        if (_count < CharBuffer.Length)
        {
            _buffer[_count++] = c;
            return;
        }

        EnsureArrayCapacity();
        _array![_count++ - CharBuffer.Length] = c;
    }

    private void EnsureArrayCapacity()
    {
        if (_array is null)
        {
            _array = ArrayPool<char>.Shared.Rent(CharBuffer.Length * 2);
        }

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
