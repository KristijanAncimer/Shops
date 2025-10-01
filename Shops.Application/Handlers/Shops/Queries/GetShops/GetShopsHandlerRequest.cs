using MediatR;
using Shops.Domain.Models;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public record GetShopsHandlerRequest(string? Filter) : IRequest<List<GetShopsHandlerDto>>
{
    public static GetShopsHandlerRequest Create(string? filter) => new(filter);
}
