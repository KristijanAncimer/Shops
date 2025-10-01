using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Application.Common;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandler : IRequestHandler<GetShopsHandlerRequest, Result<PaginatedResult<GetShopsHandlerDto>>>
{
    private readonly IAppDbContext _context;

    public GetShopsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResult<GetShopsHandlerDto>>> Handle(GetShopsHandlerRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Shops.AsQueryable();

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

        return Result<PaginatedResult<GetShopsHandlerDto>>.Success(pagedResult);
    }
}
