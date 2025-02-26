using System.Collections;

namespace Lib;

public struct SlimQueryNode
{
    private ExpressionParser _parent;
    private int _index;

    internal SlimQueryNode(ExpressionParser parent, int index)
    {
        _parent = parent;
        _index = index;
    }

    public ExpressionNodeKind Kind => _parent[_index].Kind;

    public SlimQueryNode GetLeft()
    {
        if (!this.Kind.IsBinaryOperator())
        {
            throw new InvalidOperationException("Node is not a binary operator");
        }

        return new SlimQueryNode(_parent, _parent[_index].FirstChild);
    }

    public SlimQueryNode GetRight()
    {
        if (!this.Kind.IsBinaryOperator())
        {
            throw new InvalidOperationException("Node is not a binary operator");
        }

        return new SlimQueryNode(_parent, _parent[_index].LastChild);
    }

    public ReadOnlySpan<char> GetRawValueSpan()
    {
        return _parent.GetValueSpan(_index);
    }

    public ReadOnlyMemory<char> GetRawValueMemory()
    {
        return _parent.GetValueMemory(_index);
    }

    public bool GetBoolean()
    {
        return this.Kind switch
        {
            ExpressionNodeKind.True => true,
            ExpressionNodeKind.False => false,
            _ => throw new InvalidOperationException("Node is not a boolean")
        };
    }

    public int GetInt()
    {
        if (this.Kind != ExpressionNodeKind.IntConstant)
        {
            throw new InvalidOperationException("Node is not an integer constant");
        }
        return int.Parse(GetRawValueSpan());
    }

    public string GetString()
    {
        if (this.Kind != ExpressionNodeKind.StringContant)
        {
            throw new InvalidOperationException("Node is not a string constant");
        }
        return GetRawValueSpan().ToString();
    }

    public string GetIdentifier()
    {
        if (this.Kind != ExpressionNodeKind.Identifier)
        {
            throw new InvalidOperationException("Node is not an identifier");
        }

        return GetRawValueSpan().ToString();
    }

    public int GetCount()
    {
        if (this.Kind != ExpressionNodeKind.Array)
        {
            throw new InvalidOperationException("Node is not an array");
        }

        // naive count, we should add count to expression node
        int count = 0;
        var enumerator = this.GetArrayEnumerator();
        while (enumerator.MoveNext())
        {
            count++;
        }

        return count;
    }

    public SlimQueryNode GetElement(int index)
    {
        if (this.Kind != ExpressionNodeKind.Array)
        {
            throw new InvalidOperationException("Node is not an array");
        }

        // TODO: optimize
        int pos = -1;
        var enumerator = this.GetArrayEnumerator();
        while (pos < index && enumerator.MoveNext())
        {
            pos++;
        }

        if (pos == index)
        {
            return enumerator.Current;
        }

        throw new IndexOutOfRangeException($"The index {index} was out of the bounds of the array.");
    }

    public ArrayEnumerator GetArrayEnumerator()
    {
        return new ArrayEnumerator(this);
    }

    public T Accept<T>(ISyntacticExpressionHandler<T> visitor)
    {
        return this.Kind switch
        {
            ExpressionNodeKind.Identifier => visitor.HandleIdentifier(this),
            ExpressionNodeKind.IntConstant => visitor.HandleIntConstant(this),
            ExpressionNodeKind.StringContant => visitor.HandleStringConstant(this),
            ExpressionNodeKind.Eq => visitor.HandleEq(this),
            ExpressionNodeKind.Gt => visitor.HandleGt(this),
            ExpressionNodeKind.And => visitor.HandleAnd(this),
            ExpressionNodeKind.Or => visitor.HandleOr(this),
            _ => throw new InvalidOperationException($"Unknown node kind {this.Kind}")
        };
    }

    public struct ArrayEnumerator : IEnumerator<SlimQueryNode>
    {
        private readonly SlimQueryNode _arrayNode;
        private int _curIndex;
        private readonly int _lastIndex;

        public ArrayEnumerator(SlimQueryNode arrayNode)
        {
            _arrayNode = arrayNode;
            _curIndex = -2;
            _lastIndex = _arrayNode._parent[_arrayNode._index].LastChild;
        }

        public SlimQueryNode Current
        {
            get
            {
                if (_curIndex == -1)
                {
                    return default;
                }

                return new SlimQueryNode(_arrayNode._parent, _curIndex);
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            // at beginnging _curIndex == -2,
            if (_curIndex == -2)
            {
                _curIndex = _arrayNode._parent[_arrayNode._index].FirstChild;
                return _curIndex >= 0;
            }

            // if array is empty, lastChild will be -1, return false,
            if (_curIndex == -1)
            {
                return false;
            }

            // if current item is terminal, increment index,
            int lastChild = _arrayNode._parent[_curIndex].LastChild;
            if (lastChild == -1)
            {
                _curIndex += 1;
                return _curIndex <= _lastIndex;
            }

            // if current item is non-terminal, find the end index of the
            // current item by jumping over all it's (nested) children
            int nextIndex = lastChild;
            while (lastChild != -1)
            {
                lastChild = _arrayNode._parent[nextIndex].LastChild;
                if (lastChild != -1)
                {
                    nextIndex = lastChild;
                }
            }

            _curIndex = nextIndex;

            // if _curIndex == lastChild, reached end, return false
            return _curIndex >= 0 && _curIndex <= _lastIndex;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
