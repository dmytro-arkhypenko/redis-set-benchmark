using MessagePack;
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

            int[] values = Enumerable.Range(0, SetSize).Select(_ => random.Next(1, MaxValue)).ToArray();
            var byteValue = MessagePackSerializer.Serialize(values);
            db.StringSet(key, byteValue);
        }
    }

    public HashSet<int> FetchSet(string key)
    {
        var byteValue = db.StringGet(key);
        return new HashSet<int>(MessagePackSerializer.Deserialize<int[]>(byteValue));
    }

    public TimeSpan MeasureSetOperation(Func<RedisKey[], HashSet<int>> operation, RedisKey[] keys)
    {
        var stopwatch = Stopwatch.StartNew();
        operation(keys);
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    static void Main(string[] args)
    {
        var benchmark = new RedisBenchmark("localhost:6379");
        benchmark.PopulateSets();

        int[] setCounts = { 3, 5, 7, 10, 15, 20 };

        foreach (var count in setCounts)
        {
            // Select first 'count' sets for the operation
            var keys = Enumerable.Range(1, count).Select(i => (RedisKey)$"set{i}").ToArray();

            var intersectionTime = benchmark.MeasureSetOperation(sets =>
            {
                var result = benchmark.FetchSet(sets[0]);
                for (int i = 1; i < sets.Length; i++)
                {
                    var setN = benchmark.FetchSet(sets[i]);
                    result.IntersectWith(setN);
                }
                return result;
            }, keys);
            Console.WriteLine($"Intersection Time for {count} sets: {intersectionTime}");

            var unionTime = benchmark.MeasureSetOperation(sets =>
            {
                var result = benchmark.FetchSet(sets[0]);
                for (int i = 1; i < sets.Length; i++)
                {
                    var setN = benchmark.FetchSet(sets[i]);
                    result.UnionWith(setN);
                }
                return result;
            }, keys);
            Console.WriteLine($"Union Time for {count} sets: {unionTime}");

            var differenceTime = benchmark.MeasureSetOperation(sets =>
            {
                var result = benchmark.FetchSet(sets[0]);
                for (int i = 1; i < sets.Length; i++)
                {
                    var setN = benchmark.FetchSet(sets[i]);
                    result.ExceptWith(setN);
                }
                return result;
            }, keys);
            Console.WriteLine($"Difference Time for {count} sets: {differenceTime}");

            Console.WriteLine();
        }
    }
}
