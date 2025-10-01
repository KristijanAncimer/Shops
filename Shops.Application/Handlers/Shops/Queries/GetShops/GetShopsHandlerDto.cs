namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandlerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
