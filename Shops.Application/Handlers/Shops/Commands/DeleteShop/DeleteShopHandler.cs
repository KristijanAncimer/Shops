using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using static System.Formats.Asn1.AsnWriter;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandler : IRequestHandler<DeleteShopHandlerRequest, string>
{
    private readonly IAppDbContext _context;
    public DeleteShopHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<string> Handle(DeleteShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = _context.Shops
        .FirstOrDefault(x => x.Id == request.Id);

        if (shop == null)
        {
            return $"Shop with id {request.Id} does not exist.";
        }
        _context.Shops.Remove(shop);
        await _context.SaveChangesAsync(cancellationToken);

        return $"Shop {request.Id} was deleted successfully.";
    }
}
