using SampleAnalysis.Tests;
using SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalysis.Tests;

public class ListSegmentsDataCollectionTests : DataCollectionBaseTests
{
    public ListSegmentsDataCollectionTests() : base(DataCollectionFactory.ArraySegmentsCollectionName)
    {
    }
}
