using System.Text.Json.Serialization;
using Basket.Core.BasketAggregate;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(ILogger<BasketRepository> logger, HybridCache cache) : IBasketRepository
{
    // - /basket/{id} "string" per unique basket
    private const string BasketKeyPrefix = "/basket/";

    private readonly HybridCache _cache = cache;

    private static string GetBasketKey(Guid userId) => $"{BasketKeyPrefix}{userId}";

    public async Task DeleteBasketAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(GetBasketKey(id), cancellationToken);
    }

    public async Task<CustomerBasket> GetBasketAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var basket = await _cache.GetOrCreateAsync(
            GetBasketKey(customerId),
            _ => ValueTask.FromResult(new CustomerBasket { CustomerId = customerId }),
            cancellationToken: cancellationToken);

        return basket ?? new CustomerBasket { CustomerId = customerId };
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, CancellationToken cancellationToken = default)
    {
        await _cache.SetAsync(GetBasketKey(basket.CustomerId), basket, cancellationToken: cancellationToken);

        return basket;
    }
}

[JsonSerializable(typeof(CustomerBasket))]
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.KebabCaseLower)]
public partial class BasketSerializationContext : JsonSerializerContext
{

}
