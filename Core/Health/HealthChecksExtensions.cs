using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Health;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: config.GetConnectionString("DefaultConnection")!,
                name: "Shops Database",
                tags: new[] { "db", "sql", "shops" })
            .AddSqlServer(
                connectionString: config.GetConnectionString("AuthConnection")!,
                name: "Auth Database",
                tags: new[] { "db", "sql", "auth" });

        return services;
    }
}
