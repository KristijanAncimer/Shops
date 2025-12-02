using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Core.Cache.Services;

public class ShopsCacheService : IShopsCacheService
{
    private readonly IDistributedCache _cache;
    private const string CacheKeyPrefix = "GetShops";

    public ShopsCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetCachedShopsAsync<T>(int pageNumber, int pageSize, string? filter, CancellationToken cancellationToken)
    {
        var version = await _cache.GetCurrentVersionAsync(cancellationToken);
        var cacheKey = GenerateCacheKey(version, pageNumber, pageSize, filter);

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (string.IsNullOrEmpty(cached))
            return default;

        return JsonSerializer.Deserialize<T>(cached);
    }

    public async Task SetCachedShopsAsync<T>(int pageNumber, int pageSize, string? filter, T data, CancellationToken cancellationToken)
    {
        var version = await _cache.GetCurrentVersionAsync(cancellationToken);
        var cacheKey = GenerateCacheKey(version, pageNumber, pageSize, filter);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        var serializedData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);
    }

    private static string GenerateCacheKey(string version, int pageNumber, int pageSize, string? filter)
    {
        return $"{CacheKeyPrefix}:v{version}:{pageNumber}:{pageSize}:{filter ?? "none"}";
    }
}
