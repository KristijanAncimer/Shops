using Core.Middlewares;
using ShopsApi.Util;

namespace ShopsApi.Extensions;

public static class MiddlewareConfiguration
{
    public static WebApplication ConfigureMiddleware(
        this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.UseMiddleware<PerformanceLoggingMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();

        return app;
    }

    public static async Task ConfigureDatabaseMigration(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        if (!app.Environment.IsEnvironment("Testing"))
        {
            await DatabaseMigrator.Migrate(app, cancellationToken);
        }
    }
}
