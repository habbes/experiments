using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPDemo;

internal class ProductReader
{ 
    IProductRepository products;
    IDataSource dataSource;

    public ProductReader(IDataSource dataSource, IProductRepository products)
    {
        this.products = products;
        this.dataSource = dataSource;
    }

    public void ReadData(string folder)
    {
        foreach (Product product in dataSource.ReadData())
        {
            products.Add(product);
        }
    }
    
}
