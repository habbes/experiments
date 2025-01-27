namespace SampleApi;

internal class DataStore
{
    private Dictionary<string, RecordStats> cache = new();

    public void Update(IEnumerable<LocationData> data)
    {
        lock (cache)
        {
            foreach (var item in data)
            {
                if (cache.TryGetValue(item.Name, out RecordStats stats))
                {
                    stats.Count++;
                    stats.Sum += item.Temperature;
                    stats.Max = Math.Max(stats.Max, item.Temperature);
                    stats.Min = Math.Min(stats.Min, item.Temperature);
                }
                else
                {
                    cache[item.Name] = new RecordStats
                    {
                        Count = 1,
                        Sum = item.Temperature,
                        Max = item.Temperature,
                        Min = item.Temperature
                    };
                }
            }
        }
    }

    public LocationStats GetStats(string location)
    {
        lock (cache)
        {
            if (cache.TryGetValue(location, out RecordStats stats))
            {
                return new LocationStats(location, stats.Max, stats.Min, stats.Sum / stats.Count);
            }
        }

        return new LocationStats(location, 0, 0, 0);
    }

    class RecordStats
    {
        public int Count { get; set; }
        public double Sum { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}
