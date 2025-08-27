using InlineArrayStructDotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public class HybridPooledMinArrayCharStackTests : BaseStackTests<HybridPooledMinArrayCharStack>
{
    protected override HybridPooledMinArrayCharStack CreateStack() => new();
}
