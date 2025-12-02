using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ShopsApi.Extensions;

public static class RateLimitingConfiguration
{
    private const int WindowSeconds = 10;
    private const int PermitLimit = 5;
    private const int QueueLimit = 1;

    public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromSeconds(WindowSeconds);
                limiterOptions.PermitLimit = PermitLimit;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = QueueLimit;
            });
        });

        return services;
    }
}
