using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;

namespace ShopsApi.Util;

public static class DatabaseMigrator
{
    public static async Task Migrate(WebApplication app, CancellationToken cancellationToken)
    {
        using var scope = app.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await database.Database.MigrateAsync(cancellationToken);
    }
}