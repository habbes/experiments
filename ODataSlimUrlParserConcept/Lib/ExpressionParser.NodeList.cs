using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public partial class ExpressionParser
{
    private struct NodeList : IDisposable
    {
        ExpressionNode[]? _list;
        int _length = 0;

        public NodeList(int capacity)
        {
            _list = ArrayPool<ExpressionNode>.Shared.Rent(capacity);

        }

        public void Add(ExpressionNode node)
        {
            if (_length >= _list!.Length)
            {
                Resize();
            }

            _list[_length++] = node;
        }

        public ref ExpressionNode this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                {
                    throw new IndexOutOfRangeException();
                }

                return ref _list![index];
            }
        }

        public int Count => _length;

        private void Resize()
        {
            int newLength = _list!.Length * 2;
            var newList = ArrayPool<ExpressionNode>.Shared.Rent(newLength);
            Array.Copy(_list, newList, _length);

            ArrayPool<ExpressionNode>.Shared.Return(_list);

            _list = newList;
        }

        public void Dispose()
        {
            if (_list == null)
            {
                return;
            }

            ArrayPool<ExpressionNode>.Shared.Return(_list);

            _list = null;
        }
    }
}