using Core.Middlewares.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using static System.Formats.Asn1.AsnWriter;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandler : IRequestHandler<DeleteShopHandlerRequest, Unit>
{
    private readonly IAppDbContext _context;
    public DeleteShopHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = await _context.Shops.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shop == null)
        {
            throw new NotFoundException($"Shop with id {request.Id} was not found.");
        }
        _context.Shops.Remove(shop);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
