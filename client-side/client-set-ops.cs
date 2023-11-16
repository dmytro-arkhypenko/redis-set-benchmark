using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MessagePack;
using StackExchange.Redis;

namespace client_benchmark;

[MemoryDiagnoser]
public class RedisBenchmark
{
    private ConnectionMultiplexer redis;
    private IDatabase db;
    private const int MaxValue = 100000;
    private const int SetSize = 50000;
    private const int TotalSets = 20;
    private Random random = new(123456789);
    private MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    // Serialize and compress.
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

            int[] values = Enumerable.Range(0, SetSize).Select(_ => random.Next(1, MaxValue)).ToArray();
            var byteValue = MessagePackSerializer.Serialize(values, options);
            db.StringSet(key, byteValue);
        }
    }

    private HashSet<int> FetchSet(string key)
    {
        var byteValue = db.StringGet(key);
        return new HashSet<int>(MessagePackSerializer.Deserialize<int[]>(byteValue, options));
    }

    private HashSet<int>[] FetchAllSets()
    {
        return Enumerable.Range(1, NumberOfSets)
                     .AsParallel()
                     .Select(i => FetchSet($"set{i}"))
                     .ToArray();
    }

    [Benchmark]
    public HashSet<int> MeasureSetIntersection()
    {
        var sets = FetchAllSets();
        var result = sets[0];
        for (int i = 1; i < sets.Length; i++)
        {
            result.IntersectWith(sets[i]);
        }
        return result;
    }

    [Benchmark]
    public HashSet<int> MeasureSetUnion()
    {
        var sets = FetchAllSets();
        var result = sets[0];
        for (int i = 1; i < sets.Length; i++)
        {
            result.UnionWith(sets[i]);
        }
        return result;
    }

    public static HashSet<T> ParallelUnionSets<T>(IEnumerable<HashSet<T>> listOfSets)
    {
        return listOfSets
            .AsParallel()
            .Aggregate(new HashSet<T>(),
                       (accumulatedSet, nextSet) =>
                       {
                           accumulatedSet.UnionWith(nextSet);
                           return accumulatedSet;
                       });
    }

    [Benchmark]
    public HashSet<int> MeasureSetUnionParallel2()
    {
        var sets = FetchAllSets();
        return ParallelUnionSets(sets);
    }

    [Benchmark]
    public int[] MeasureSetUnionParallel()
    {
        var sets = FetchAllSets();
        ConcurrentHashset _Data = new(sets[0]);
        Parallel.For(1, sets.Length, i => _Data.UnionWith(sets[i]));
        return _Data.GetData();
    }

    [Benchmark]
    public HashSet<int> MeasureSetDifference()
    {
        var sets = FetchAllSets();
        var result = sets[0];
        for (int i = 1; i < sets.Length; i++)
        {
            result.ExceptWith(sets[i]);
        }
        return result;
    }
}

public class Program
{
    public static void Main(string[] args)
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
