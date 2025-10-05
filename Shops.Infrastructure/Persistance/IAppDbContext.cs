using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shops.Domain.Models;

namespace Shops.Infrastructure.Persistance;

public interface IAppDbContext
{
    DbSet<Shop> Shops { get; }
    ChangeTracker ChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
