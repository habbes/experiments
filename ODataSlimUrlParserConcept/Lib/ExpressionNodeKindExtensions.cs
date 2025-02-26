using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public static class ExpressionNodeKindExtensions
{
    public static bool IsBinaryOperator(this ExpressionNodeKind kind)
    {
        // TODO: we should reserve some bits in the enum to store this information
        return kind switch
        {
            ExpressionNodeKind.Eq => true,
            ExpressionNodeKind.Gt => true,
            ExpressionNodeKind.And => true,
            ExpressionNodeKind.Or => true,
            ExpressionNodeKind.In => true,
            _ => false,
        };
    }
}
