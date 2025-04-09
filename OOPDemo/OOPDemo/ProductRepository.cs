
namespace OOPDemo;

internal class ProductRepository : IProductRepository
{
    private Dictionary<int, Product> products = new();
    private static int NextId = 1;
    public Product Add(Product product)
    {
        product.Id = NextId++;
        products[product.Id] = product;
        return product;
    }

    public Product GetProduct(int id)
    {
        if (products.TryGetValue(id, out Product? product))
        {
            return product;
        }

        throw new Exception($"Product with id {id} does not exist");
    }

    public IEnumerable<Product> ListProducts()
    {
        return products.Values;
    }
}
