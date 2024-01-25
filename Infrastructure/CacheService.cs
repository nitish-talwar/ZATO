using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ZATO.Abstractions;

namespace ZATO.Infrastructure;

public class CacheService : ICacheService
{
    public static ConcurrentDictionary<string, bool> cacheKeys = new();
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(
            key,
            cancellationToken);
        if (cachedValue is null)
        {
            return null;
        }

        T? value = JsonConvert.DeserializeObject<T>(cachedValue);
        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cachedValue = JsonConvert.SerializeObject(value);
        await _distributedCache.SetStringAsync(key, cachedValue, cancellationToken);
        cacheKeys.TryAdd(key, false);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
        cacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        // foreach (string key in cacheKeys.Keys)
        // {
        //     if (key.StartsWith(prefixKey))
        //     {
        //         await RemoveAsync(key, cancellationToken);
        //     }
        // }
        IEnumerable<Task> tasks = cacheKeys.Keys.Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));
        await Task.WhenAll(tasks);
    }
}