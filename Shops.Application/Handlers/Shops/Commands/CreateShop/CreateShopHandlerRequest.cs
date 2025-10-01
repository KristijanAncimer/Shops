using MediatR;
using Shops.Application.Common;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandlerRequest : IRequest<Result<CreateShopDto>>
{
    public string Name { get; set; } = string.Empty;
    public CreateShopHandlerRequest(string name)
    {
        Name = name;
    }
    public static CreateShopHandlerRequest Create(string name)
        => new CreateShopHandlerRequest(name);
}