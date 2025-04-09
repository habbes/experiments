using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal class CompositeFileHandler : IFileHandler
    {
        List<IFileHandler> handlers = new();

        public void Add(IFileHandler handler)
        {
            handlers.Add(handler);
        }
        public bool CanRead(string path)
        {
            return handlers.Any(handler => handler.CanRead(path));
        }

        public IEnumerable<Product> ReadData(string path)
        {
            var handler = handlers.First(handler => handler.CanRead(path));
            return handler.ReadData(path);
        }
    }
}
