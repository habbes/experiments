using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal class JsonDataHandler : IFileHandler
    {
        public bool CanRead(string path)
        {
            return path.EndsWith("json");
        }

        public IEnumerable<Product> ReadData(string path)
        {
            var result = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(path));
            return result;
        }
    }
}
