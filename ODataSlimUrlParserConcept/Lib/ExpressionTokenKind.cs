using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public enum ExpressionTokenKind
{
    Identifier,
    Keyword,
    StringLiteral,
    IntLiteral,
    None
}