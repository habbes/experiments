using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo
{
    internal class FolderDataSource : IDataSource
    {
        string folderPath;
        IFileHandler fileHandler;

        public FolderDataSource(string folderPath, IFileHandler fileHandler)
        {
            this.folderPath = folderPath;
            this.fileHandler = fileHandler;
        }

        public IEnumerable<Product> ReadData()
        {
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {
                if (!this.fileHandler.CanRead(file))
                {
                    Console.WriteLine($"Unsupported file {file}");
                    continue;
                }

                foreach (var product in fileHandler.ReadData(file))
                {
                    yield return product;
                }
            }
        }
    }
}
