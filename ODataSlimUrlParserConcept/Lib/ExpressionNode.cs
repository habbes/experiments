using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

internal struct ExpressionNode
{
    public ExpressionNode(ExpressionNodeKind kind, ValueRange range)
        : this(kind, range, -1, -1)
    {
    }

    public ExpressionNode(ExpressionNodeKind kind, ValueRange range, int firstChild, int lastChild)
    {
        Kind = kind;
        Range = range;
        FirstChild = firstChild;
        LastChild = lastChild;
    }

    public ExpressionNodeKind Kind { get; internal set; }
    public ValueRange Range { get; internal set; }

    /// <summary>
    /// Index of the first child of the node if the node has children,
    /// otherwise it's -1.
    /// </summary>
    internal int FirstChild { get; set; }

    /// <summary>
    /// Index of the last child of the node if the node has children,
    /// otherwise -1.
    /// </summary>
    internal int LastChild { get; set; }

    public bool IsTerminal => FirstChild == -1;
}

