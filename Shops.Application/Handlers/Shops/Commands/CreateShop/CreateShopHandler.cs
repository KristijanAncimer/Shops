using AutoMapper;
using Core.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shops.Application.Common;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandler : IRequestHandler<CreateShopHandlerRequest, Result<CreateShopDto>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<CreateShopHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public CreateShopHandler(IAppDbContext context, ILogger<CreateShopHandler> logger, IMapper mapper, IDistributedCache cache)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<CreateShopDto>> Handle(CreateShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = new Shop
        {
            Name = request.Name
        };

        await _context.Shops.AddAsync(shop, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.IncrementVersionAsync(cancellationToken);

        var dto = _mapper.Map<CreateShopDto>(shop);
        _logger.LogInformation("Shop created with {@Dto}", dto);
        return Result<CreateShopDto>.Success(dto);
    }
}
