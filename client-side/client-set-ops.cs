using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MessagePack;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

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
        redis = ConnectionMultiplexer.Connect("crunchy-redis:6379");
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
            var byteValue = MessagePackSerializer.Serialize(values);
            db.StringSet(key, byteValue);
        }
    }

    private HashSet<int> FetchSet(string key)
    {
        var byteValue = db.StringGet(key);
        return new HashSet<int>(MessagePackSerializer.Deserialize<int[]>(byteValue));
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
        var config = ManualConfig.Create(DefaultConfig.Instance)
            .With(Job.Default
                .WithWarmupCount(1)    // Number of warmup iterations
                .WithIterationCount(3) // Number of target iterations
            );

        BenchmarkRunner.Run<RedisBenchmark>(config);
    }
}
