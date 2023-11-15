using StackExchange.Redis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

public class RedisBenchmark
{
    private readonly ConnectionMultiplexer redis;
    private readonly IDatabase db;
    private const int MaxValue = 100000;
    private const int SetSize = 50000;
    private const int TotalSets = 20;
    private Random random = new Random(123456789);

    [Params(3, 5, 7, 10, 15, 20)]
    public int NumberOfSets { get; set; }

    public RedisBenchmark()
    {
        redis = ConnectionMultiplexer.Connect("localhost:6379");
        db = redis.GetDatabase();
        PopulateSets();
    }

    private void PopulateSets()
    {
        for (int i = 1; i <= TotalSets; i++)
        {
            var key = $"set{i}";
            db.KeyDelete(key);
            RedisValue[] values = Enumerable.Range(0, SetSize).Select(_ => (RedisValue)(random.Next(1, MaxValue))).ToArray();
            db.SetAdd(key, values);
        }
    }

    [Benchmark]
    public void MeasureSetIntersection()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        db.SetCombine(SetOperation.Intersect, keys);
    }

    [Benchmark]
    public void MeasureSetUnion()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        db.SetCombine(SetOperation.Union, keys);
    }

    [Benchmark]
    public void MeasureSetDifference()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        db.SetCombine(SetOperation.Difference, keys);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var config = ManualConfig.Create(DefaultConfig.Instance)
            .With(Job.Default
                .WithWarmupCount(1)    // Number of warmup iterations
                .WithIterationCount(3) // Number of target iterations
            );

        BenchmarkRunner.Run<RedisBenchmark>(config);

    }
}
