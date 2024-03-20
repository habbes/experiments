using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalysis.Tests;

public class ArrayListDataCollectionTests : DataCollectionBaseTests
{
    public ArrayListDataCollectionTests(): base(DataCollectionFactory.ArrayListCollectionName)
    {
    }
}
