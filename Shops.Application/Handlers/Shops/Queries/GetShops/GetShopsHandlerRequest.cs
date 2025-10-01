using MediatR;
using Shops.Application.Common;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandlerRequest : IRequest<Result<PaginatedResult<GetShopsHandlerDto>>>
{
    public string? Filter { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetShopsHandlerRequest(string? filter, int? pageNumber = 1, int? pageSize = 10)
    {
        Filter = filter;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
    }
    public static GetShopsHandlerRequest Create(string? filter, int? pageNumber, int? pageSize)
        => new(filter, pageNumber, pageSize);
}
