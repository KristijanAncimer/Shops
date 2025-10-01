using MediatR;
using Microsoft.Extensions.Logging;
using Shops.Application.Common;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandler : IRequestHandler<CreateShopHandlerRequest, Result<CreateShopDto>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<CreateShopHandler> _logger;
    public CreateShopHandler(IAppDbContext context, ILogger<CreateShopHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<CreateShopDto>> Handle(CreateShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = new Shop
        {
            Name = request.Name
        };

        await _context.Shops.AddAsync(shop, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = new CreateShopDto
        {
            Id = shop.Id,
            Name = shop.Name
        };
        _logger.LogInformation("Shop created with {@Dto}", dto);
        return Result<CreateShopDto>.Success(dto);
    }
}
