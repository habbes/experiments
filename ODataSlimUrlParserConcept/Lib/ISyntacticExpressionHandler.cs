using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public interface ISyntacticExpressionHandler<T>
{
    T HandleIdentifier(SlimQueryNode identifier);
    T HandleIntConstant(SlimQueryNode intConst);
    T HandleStringConstant(SlimQueryNode stringConst);
    T HandleBoolConstant(SlimQueryNode boolConst);
    T HandleArray(SlimQueryNode arrayNode);
    T HandleEq(SlimQueryNode eqNode);
    T HandleGt(SlimQueryNode gtNode);
    T HandleOr(SlimQueryNode orNode);
    T HandleAnd(SlimQueryNode andNode);
    T HandleIn(SlimQueryNode inNode);
}

