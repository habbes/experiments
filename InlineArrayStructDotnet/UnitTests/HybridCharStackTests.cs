using InlineArrayStructDotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public class HybridCharStackTests : BaseStackTests<HybridCharStack>
{
    protected override HybridCharStack CreateStack() => new();
}
