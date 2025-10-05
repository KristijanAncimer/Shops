using AutoMapper;
using Core.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shops.Application.Common;
using Shops.Domain.Models;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Commands.UpdateShop;

public class UpdateShopHandler : IRequestHandler<UpdateShopHandlerRequest, Result<UpdateShopHandlerDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public UpdateShopHandler(IAppDbContext context, IMapper mapper, IDistributedCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }


    public async Task<Result<UpdateShopHandlerDto>> Handle(UpdateShopHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = new Shop { Id = request.Id };
        _context.Shops.Attach(shop);

        shop.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        await _cache.IncrementVersionAsync(cancellationToken);

        var dto = _mapper.Map<UpdateShopHandlerDto>(shop);

        return Result<UpdateShopHandlerDto>.Success(dto);
    }
}