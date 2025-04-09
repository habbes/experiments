using SampleAnalysis.Tests;
using SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalysis.Tests;

public class DictionaryDataCollectionTests : DataCollectionBaseTests
{
    public DictionaryDataCollectionTests() : base(DataCollectionFactory.DictionaryCollectionName)
    {
    }
}
