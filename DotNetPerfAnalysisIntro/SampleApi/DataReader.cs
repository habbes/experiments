namespace SampleApi;

internal class DataReader
{
    public static async Task<List<LocationData>> ReadDataAsync(Stream input)
    {
        List<LocationData> data = new();
        var reader = new StreamReader(input);

        string content = await reader.ReadToEndAsync();

        var lines = content.Split(Environment.NewLine);

        foreach (var line in lines)
        {
            var words = line.Split(" ");
            string name = default;
            double temperature = default;
            foreach (var word in words)
            {
                if (word.StartsWith("location:"))
                {
                    name = word.Split(':')[1];
                }
                else if (word.StartsWith("temp:"))
                {
                    temperature = double.Parse(word.Split(':')[1]);
                }
            }

            var locationData = new LocationData(name!, temperature);
            data.Add(locationData);
        }

        return data;
    }
}
