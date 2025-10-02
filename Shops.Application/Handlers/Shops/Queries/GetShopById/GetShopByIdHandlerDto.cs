namespace Shops.Application.Handlers.Shops.Queries.GetShopById;

public class GetShopByIdHandlerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
