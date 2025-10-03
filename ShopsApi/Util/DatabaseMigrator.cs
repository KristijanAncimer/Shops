using Auth.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;

namespace ShopsApi.Util;

public static class DatabaseMigrator
{
    public static async Task Migrate(WebApplication app, CancellationToken cancellationToken)
    {
        using var scope = app.Services.CreateScope();

        var shopDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await shopDb.Database.MigrateAsync(cancellationToken);

        // this is just temporary and should be moved later on but for now lets just leave it here for ease
        var authDb = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await authDb.Database.MigrateAsync(cancellationToken);

    }
}