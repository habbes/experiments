using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal interface IFileHandler
    {
        bool CanRead(string path);
        IEnumerable<Product> ReadData(string path);
    }
}
