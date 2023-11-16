using StackExchange.Redis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

[MemoryDiagnoser]
public class RedisBenchmark
{
    private ConnectionMultiplexer redis;
    private IDatabase db;
    private const int MaxValue = 100000;
    private const int SetSize = 50000;
    private const int TotalSets = 20;
    private Random random = new Random(123456789);

    [Params(3, 5, 7, 10, 15, 20)]
    public int NumberOfSets { get; set; }


    [GlobalSetup]
    public void Setup()
    {
        var redisEndpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT");
        redis = ConnectionMultiplexer.Connect(redisEndpoint);
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
    public RedisValue[] MeasureSetIntersection()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        return db.SetCombine(SetOperation.Intersect, keys);
    }

    [Benchmark]
    public RedisValue[] MeasureSetUnion()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        return db.SetCombine(SetOperation.Union, keys);
    }

    [Benchmark]
    public RedisValue[] MeasureSetDifference()
    {
        var keys = Enumerable.Range(1, NumberOfSets).Select(i => (RedisKey)$"set{i}").ToArray();
        return db.SetCombine(SetOperation.Difference, keys);
    }
}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the Redis endpoint as a command-line argument.");
            return;
        }

        Environment.SetEnvironmentVariable("REDIS_ENDPOINT", args[0], EnvironmentVariableTarget.Process);

        var config = ManualConfig.Create(DefaultConfig.Instance)
            .With(Job.Default
                .WithWarmupCount(1)    // Number of warmup iterations
                .WithIterationCount(3) // Number of target iterations
            );

        BenchmarkRunner.Run<RedisBenchmark>(config);

    }
}
