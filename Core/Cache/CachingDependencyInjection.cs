using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Cache;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddAppRedisCache(this IServiceCollection services, IConfiguration config)
    {
        var redisConfig = config.GetSection("Redis");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig["Configuration"];
            options.InstanceName = redisConfig["InstanceName"];
        });

        return services;
    }
}
