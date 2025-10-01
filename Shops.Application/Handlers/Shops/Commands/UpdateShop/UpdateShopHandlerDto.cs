namespace Shops.Application.Handlers.Shops.Commands.UpdateShop;

public class UpdateShopHandlerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
