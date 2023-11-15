using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Linq;

class RedisBenchmark
{
    private readonly ConnectionMultiplexer redis;
    private readonly IDatabase db;
    private const int MaxValue = 100000;
    private const int SetSize = 50000;
    private const int TotalSets = 20;
    private Random random = new Random();

    public RedisBenchmark(string connectionString)
    {
        redis = ConnectionMultiplexer.Connect(connectionString);
        db = redis.GetDatabase();
    }

    public void PopulateSets()
    {
        for (int i = 1; i <= TotalSets; i++)
        {
            var key = $"set{i}";
            db.KeyDelete(key);

            RedisValue[] values = Enumerable.Range(0, SetSize).Select(_ => (RedisValue)(random.Next(1, MaxValue))).ToArray();

            db.SetAdd(key, values);
        }
    }

    public TimeSpan MeasureSetOperation(Func<RedisValue[]> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        operation();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    static void Main(string[] args)
    {
        var benchmark = new RedisBenchmark("localhost:6379");
        benchmark.PopulateSets();

        // Measure for 3, 5, and 7 sets
        int[] setCounts = { 3, 5, 7, 10, 15, 20 };

        foreach (var count in setCounts)
        {
            // Select first 'count' sets for the operation
            var keys = Enumerable.Range(1, count).Select(i => (RedisKey)$"set{i}").ToArray();

            var intersectionTime = benchmark.MeasureSetOperation(() => benchmark.db.SetCombine(SetOperation.Intersect, keys));
            Console.WriteLine($"Intersection Time for {count} sets: {intersectionTime}");

            var unionTime = benchmark.MeasureSetOperation(() => benchmark.db.SetCombine(SetOperation.Union, keys));
            Console.WriteLine($"Union Time for {count} sets: {unionTime}");

            var differenceTime = benchmark.MeasureSetOperation(() => benchmark.db.SetCombine(SetOperation.Difference, keys));
            Console.WriteLine($"Difference Time for {count} sets: {differenceTime}\n");
        }
    }
}
