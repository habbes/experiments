using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal interface IDataSource
    {
        IEnumerable<Product> ReadData();
    }
}
