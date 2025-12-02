using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Cache.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Application.Common;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandler : IRequestHandler<GetShopsHandlerRequest, Result<PaginatedResult<GetShopsHandlerDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IShopsCacheService _cacheService;
    private readonly IMapper _mapper;

    public GetShopsHandler(IAppDbContext context, IShopsCacheService cacheService, IMapper mapper)
    {
        _context = context;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<GetShopsHandlerDto>>> Handle(GetShopsHandlerRequest request, CancellationToken cancellationToken)
    {
        var cachedResult = await _cacheService.GetCachedShopsAsync<PaginatedResult<GetShopsHandlerDto>>(
            request.PageNumber,
            request.PageSize,
            request.Filter,
            cancellationToken);

        if (cachedResult != null)
            return Result<PaginatedResult<GetShopsHandlerDto>>.Success(cachedResult);

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

        await _cacheService.SetCachedShopsAsync(
            request.PageNumber,
            request.PageSize,
            request.Filter,
            pagedResult,
            cancellationToken);

        return Result<PaginatedResult<GetShopsHandlerDto>>.Success(pagedResult);
    }
}
