using MediatR;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandlerRequest : IRequest<CreateShopDto>
{
    public string Name { get; set; } = string.Empty;
    public CreateShopHandlerRequest(string name)
    {
        Name = name;
    }
    public static CreateShopHandlerRequest Create(string name)
        => new CreateShopHandlerRequest(name);
}
