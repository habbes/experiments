using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;


[InlineArray(CharBuffer.Length)]
internal struct CharBuffer
{
    public const int Length = 64;
    private char _element;
}