using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;

namespace Shops.Infrastructure.Persistance;

public interface IAppDbContext
{
    DbSet<Shop> Shops { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
