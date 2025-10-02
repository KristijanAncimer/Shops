using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shops.Application.Common;
using Shops.Infrastructure.Persistance;
using System.Text.Json;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandler : IRequestHandler<GetShopsHandlerRequest, Result<PaginatedResult<GetShopsHandlerDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IDistributedCache _cache;

    public GetShopsHandler(IAppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<PaginatedResult<GetShopsHandlerDto>>> Handle(GetShopsHandlerRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{request.PageNumber}_{request.PageSize}_{request.Filter}";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cached))
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<PaginatedResult<GetShopsHandlerDto>>(cached);
            return Result<PaginatedResult<GetShopsHandlerDto>>.Success(result!);
        }

        var query = _context.Shops.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            query = query.Where(x => x.Name.Contains(request.Filter));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new GetShopsHandlerDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            })
            .ToListAsync(cancellationToken);

        var pagedResult = new PaginatedResult<GetShopsHandlerDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(pagedResult),
            cacheOptions,
            cancellationToken
        );

        return Result<PaginatedResult<GetShopsHandlerDto>>.Success(pagedResult);
    }
}
