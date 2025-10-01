using MediatR;
using Shops.Application.Common;

namespace Shops.Application.Handlers.Shops.Queries.GetShopById;

public class GetShopByIdHandlerRequest : IRequest<Result<GetShopByIdHandlerDto>>
{
    public int Id { get; set; }
    public GetShopByIdHandlerRequest(int id)
    {
        Id = id;
    }
    public static GetShopByIdHandlerRequest Create(int id)
        => new(id);
}
