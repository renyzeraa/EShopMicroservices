using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data;

public class CacheBasketRepository
  (IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
  private readonly IBasketRepository repository = repository;
  private readonly IDistributedCache cache = cache;
  public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
  {
    var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

    if (!string.IsNullOrEmpty(cachedBasket))
      return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

    var storedBasket = await repository.GetBasket(userName, cancellationToken);
    await cache.SetStringAsync(userName, JsonSerializer.Serialize(storedBasket), cancellationToken);

    return storedBasket;
  }

  public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
  {
    await repository.StoreBasket(basket, cancellationToken);
    await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

    return basket;
  }

  public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
  {
    await repository.DeleteBasket(userName, cancellationToken);
    await cache.RemoveAsync(userName, cancellationToken);

    return true;
  }
}