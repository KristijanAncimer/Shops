using MediatR;
using Shops.Application.Common;
using System.Text.Json.Serialization;

namespace Shops.Application.Handlers.Shops.Commands.UpdateShop;

public class UpdateShopHandlerRequest : IRequest<Result<UpdateShopHandlerDto>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UpdateShopHandlerRequest(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public static UpdateShopHandlerRequest Create(int id, string name) => new(id, name);
}
