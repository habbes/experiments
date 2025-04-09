using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal class CsvFileHandler : IFileHandler
    {
        public bool CanRead(string path)
        {
            return path.EndsWith("csv");
        }

        public IEnumerable<Product> ReadData(string path)
        {
            return ReadDataFromFile(path);
        }

        private IEnumerable<Product> ReadDataFromFile(string filename)
        {
            var reader = new StreamReader(filename);

            string? line = reader.ReadLine();
            // skip row header line
            line = reader.ReadLine();
            while (line != null)
            {
                Product product = ReadProduct(line);
                yield return product;
                line = reader.ReadLine();
            }
        }

        private Product ReadProduct(string line)
        {
            var parts = line.Split(",");
            var product = new Product
            {
                ItemCode = parts[0],
                Name = parts[1],
                Description = parts[2],
                Price = decimal.Parse(parts[3])
            };

            return product;
        }
    }
}
