using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public struct StandardCharStack : ICharStack
{
    Stack<char> _stack;

    public StandardCharStack()
    {
        _stack = new Stack<char>();
    }

    public StandardCharStack(int capacity)
    {
        _stack = new Stack<char>(capacity);
    }
    public int Count => _stack.Count;

    public char Pop() => _stack.Pop();

    public void Push(char c) => _stack.Push(c);
}
