using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Application.Common;
using Shops.Domain.Models;

namespace Shops.Application.Handlers.Shops.Commands.UpdateShop;

public class UpdateShopHandler : IRequestHandler<UpdateShopHandlerRequest, Result<UpdateShopHandlerDto>>
{
    private readonly AppDbContext _context;

    public UpdateShopHandler(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Result<UpdateShopHandlerDto>> Handle(UpdateShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = await _context.Shops
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shop == null)
        {
            return Result<UpdateShopHandlerDto>.Failure($"Shop with id {request.Id} does not exist.");
        }

        shop.Name = request.Name;
        shop.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var dto = new UpdateShopHandlerDto
        {
            Id = shop.Id,
            Name = shop.Name,
            UpdatedAt = shop.UpdatedAt,
            CreatedAt = shop.CreatedAt,
        };
        return Result<UpdateShopHandlerDto>.Success(dto);
    }
}