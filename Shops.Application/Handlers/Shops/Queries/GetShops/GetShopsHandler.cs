using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Cache;
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
    private readonly IMapper _mapper;

    public GetShopsHandler(IAppDbContext context, IDistributedCache cache, IMapper mapper)
    {
        _context = context;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<GetShopsHandlerDto>>> Handle(GetShopsHandlerRequest request, CancellationToken cancellationToken)
    {
        //var cacheKey = $"GetShops:{request.PageNumber}:{request.PageSize}:{request.Filter ?? "none"}";
        var version = await _cache.GetCurrentVersionAsync(cancellationToken);
        var cacheKey = $"GetShops:v{version}:{request.PageNumber}:{request.PageSize}:{request.Filter ?? "none"}";

        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cached))
        {
            var cachedResult = JsonSerializer.Deserialize<PaginatedResult<GetShopsHandlerDto>>(cached);
            return Result<PaginatedResult<GetShopsHandlerDto>>.Success(cachedResult!);
        }

        var query = _context.Shops.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Filter))
            query = query.Where(x => x.Name.Contains(request.Filter));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<GetShopsHandlerDto>(_mapper.ConfigurationProvider)
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
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
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
