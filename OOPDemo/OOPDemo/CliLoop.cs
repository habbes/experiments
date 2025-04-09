namespace OOPDemo;

internal class CliLoop
{
    IProductRepository productRepository;

    public CliLoop(IProductRepository repository)
    {
        productRepository = repository;
    }

    public void Start()
    {
        Console.WriteLine("Welcome to the product database");
        
        while (true)
        {
            Console.WriteLine("Enter command (list, get, search)");
            var commandLine = Console.ReadLine();
            if (commandLine == null)
            {
                continue;
            }
            var parts = commandLine.Split(' ');
            var command = parts[0];
            var arg = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case "list":
                    HandleListCommand();
                    break;
                case "search":
                    HandleSearchCommand(arg);
                    break;
                case "get":
                    HandleGetCommand(arg);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }

    private void HandleListCommand()
    {
        foreach (var product in productRepository.ListProducts())
        {
            PrintProduct(product);
        }
    }

    private void HandleSearchCommand(string query)
    {

    }

    private void HandleGetCommand(string id)
    {
        try
        {
            var product = productRepository.GetProduct(int.Parse(id));
            PrintProduct(product);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void PrintProduct(Product product)
    {
        Console.WriteLine($"ID={product.Id},Name={product.Name},Price={product.Price},Code={product.ItemCode},Description={product.Description}");
    }
}
