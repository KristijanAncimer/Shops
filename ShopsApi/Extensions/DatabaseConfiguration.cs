using Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using Core.Cache;

namespace ShopsApi.Extensions;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        if (!environment.IsEnvironment("Testing"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("Shops.Infrastructure"))
                .EnableSensitiveDataLogging()
                .LogTo(Log.Information, LogLevel.Information)
            );

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AuthConnection")));

            services.AddAppRedisCache(configuration);
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}
