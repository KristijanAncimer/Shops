using Core.Cache;
using Core.Middlewares.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandler : IRequestHandler<DeleteShopHandlerRequest, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IDistributedCache _cache;
    public DeleteShopHandler(IAppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }
    public async Task<Unit> Handle(DeleteShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = new Shop { Id = request.Id };

        _context.Shops.Attach(shop);
        _context.Shops.Remove(shop);

        var affected = await _context.SaveChangesAsync(cancellationToken);

        await _cache.IncrementVersionAsync(cancellationToken);

        if (affected == 0)
            throw new NotFoundException($"Shop with id {request.Id} was not found.");

        return Unit.Value;
    }
}
