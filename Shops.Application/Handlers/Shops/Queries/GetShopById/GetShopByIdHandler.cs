using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Application.Common;
using Shops.Domain.Models;

namespace Shops.Application.Handlers.Shops.Queries.GetShopById;

public class GetShopByIdHandler : IRequestHandler<GetShopByIdHandlerRequest, Result<GetShopByIdHandlerDto>>
{
    private readonly AppDbContext _context;
    public GetShopByIdHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Result<GetShopByIdHandlerDto>> Handle(GetShopByIdHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = await _context.Shops
            .Where(s => s.Id == request.Id)
            .Select(s => new GetShopByIdHandlerDto {
                Name = s.Name,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt})
            .FirstOrDefaultAsync(cancellationToken);

        if (shop is null) return Result<GetShopByIdHandlerDto>.Failure($"Shop with id {request.Id} was not found.");

        return Result<GetShopByIdHandlerDto>.Success(shop);
    }
}
