using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandler : IRequestHandler<GetShopsHandlerRequest, List<GetShopsHandlerDto>>
{
    private readonly AppDbContext _context;

    public GetShopsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetShopsHandlerDto>> Handle(GetShopsHandlerRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Shops.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            query = query.Where(x => x.Name.Contains(request.Filter));
        }

        return await query
            .Select(x => new GetShopsHandlerDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            })
            .ToListAsync(cancellationToken);
    }
}
