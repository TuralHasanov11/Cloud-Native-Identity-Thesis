using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Core.BasketAggregate;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(
    ILogger<BasketRepository> logger,
    IDistributedCache cache) : IBasketRepository
{
    // - /basket/{id} "string" per unique basket
    private const string BasketKeyPrefix = "/basket/";

    private static string GetBasketKey(string userId) => $"{BasketKeyPrefix}{userId}";

    public async Task DeleteBasketAsync(string customerId, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(GetBasketKey(customerId), cancellationToken);
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var basketString = await cache.GetStringAsync(GetBasketKey(customerId), cancellationToken);
        var basket = string.IsNullOrEmpty(basketString)
            ? null
            : JsonSerializer.Deserialize(basketString, BasketSerializationContext.Default.CustomerBasket);


        return basket ?? new CustomerBasket { CustomerId = customerId };
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, CancellationToken cancellationToken = default)
    {
        var basketString = JsonSerializer.Serialize(basket, BasketSerializationContext.Default.CustomerBasket);
        await cache.SetStringAsync(
            GetBasketKey(basket.CustomerId),
            basketString,
            cancellationToken);

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
