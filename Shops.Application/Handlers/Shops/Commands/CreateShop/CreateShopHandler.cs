using Azure.Core;
using MediatR;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;
using System;
using System.Threading;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandler : IRequestHandler<CreateShopHandlerRequest, CreateShopDto>
{
    private readonly IAppDbContext _context;
    public CreateShopHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateShopDto> Handle(CreateShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = new Shop
        {
            Name = request.Name
        };

        _context.Shops.Add(shop);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateShopDto
        {
            Id = shop.Id,
            Name = shop.Name
        };
    }
}
