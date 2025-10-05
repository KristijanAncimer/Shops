using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shops.Application.Common;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Queries.GetShopById;

public class GetShopByIdHandler : IRequestHandler<GetShopByIdHandlerRequest, Result<GetShopByIdHandlerDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    public GetShopByIdHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<GetShopByIdHandlerDto>> Handle(GetShopByIdHandlerRequest request, CancellationToken cancellationToken)
    {
        var shop = await _context.Shops.AsNoTracking()
            .Where(s => s.Id == request.Id)
            .ProjectTo<GetShopByIdHandlerDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (shop is null)
            return Result<GetShopByIdHandlerDto>.Failure($"Shop with id {request.Id} was not found.");

        return Result<GetShopByIdHandlerDto>.Success(shop);
    }
}
