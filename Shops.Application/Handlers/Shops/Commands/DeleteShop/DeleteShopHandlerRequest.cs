using MediatR;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandlerRequest : IRequest<string>
{
    public int Id { get; set; }
    public DeleteShopHandlerRequest(int id)
    {
        Id = id;
    }
    public static DeleteShopHandlerRequest Create(int id)
        => new(id);
}
