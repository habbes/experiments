using InlineArrayStructDotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public class HybridListCharStackTests : BaseStackTests<HybridListCharStack>
{
    protected override HybridListCharStack CreateStack() => new();
}
