namespace Core.Cache.Services;

public interface IShopsCacheService
{
    Task<T?> GetCachedShopsAsync<T>(int pageNumber, int pageSize, string? filter, CancellationToken cancellationToken);

    Task SetCachedShopsAsync<T>(int pageNumber, int pageSize, string? filter, T data, CancellationToken cancellationToken);
}
