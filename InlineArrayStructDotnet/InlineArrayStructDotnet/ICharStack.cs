using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public interface ICharStack
{
    void Push(char c);
    char Pop();
    int Count { get; }
}
