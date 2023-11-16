using System.Collections.Concurrent;

namespace client_benchmark;

public class ConcurrentHashset
{
    private readonly ConcurrentDictionary<int, bool> _Data = new();
    public ConcurrentHashset(IEnumerable<int> data)
    {
        foreach (var item in data)
        {
            _Data.TryAdd(item, true);
        }
    }

    public void UnionWith(IEnumerable<int> data)
    {
        foreach (var item in data)
        {
            _Data.TryAdd(item, true);
        }
    }

    public int[] GetData()
    {
        return _Data.Keys.ToArray();
    }
}
