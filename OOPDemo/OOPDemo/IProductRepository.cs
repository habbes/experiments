namespace OOPDemo;

internal interface IProductRepository
{
    public Product Add(Product product);

    public Product GetProduct(int id);

    public IEnumerable<Product> ListProducts();
}
