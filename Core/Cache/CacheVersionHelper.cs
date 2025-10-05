using Microsoft.Extensions.Caching.Distributed;

namespace Core.Cache;

public static class CacheVersionHelper
{
    private const string VersionKey = "GetShops:Version";

    public static async Task<string> GetCurrentVersionAsync(this IDistributedCache cache, CancellationToken cancellationToken)
    {
        var version = await cache.GetStringAsync(VersionKey, cancellationToken);
        if (string.IsNullOrEmpty(version))
        {
            version = "1";
            await cache.SetStringAsync(VersionKey, version, cancellationToken);
        }

        return version;
    }

    public static async Task IncrementVersionAsync(this IDistributedCache cache, CancellationToken cancellationToken)
    {
        var current = await cache.GetCurrentVersionAsync(cancellationToken);
        if (int.TryParse(current, out int version))
        {
            version++;
            await cache.SetStringAsync(VersionKey, version.ToString(), cancellationToken);
            Console.WriteLine($"🔄 Cache version incremented to v{version}");
        }
    }
}
