using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Domain.Models;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandler : IRequestHandler<DeleteShopHandlerRequest, string>
{
    private readonly AppDbContext _context;
    public DeleteShopHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<string> Handle(DeleteShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var powerPlant = await _context.Shops.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (powerPlant == null)
        {
            return $"Shop with id {request.Id} does not exist.";
        }
        _context.Shops.Remove(powerPlant);
        await _context.SaveChangesAsync(cancellationToken);

        return $"Shop {request.Id} was deleted successfully.";
    }
}
