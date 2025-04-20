using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Core.BasketAggregate;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(
    ILogger<BasketRepository> logger,
    //HybridCache cache,
    IDistributedCache cache) : IBasketRepository
{
    // - /basket/{id} "string" per unique basket
    private const string BasketKeyPrefix = "/basket/";

    //private readonly HybridCache _cache = cache;
    private readonly IDistributedCache _cache = cache;


    private static string GetBasketKey(string userId) => $"{BasketKeyPrefix}{userId}";

    public async Task DeleteBasketAsync(string customerId, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(GetBasketKey(customerId), cancellationToken);
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId, CancellationToken cancellationToken = default)
    {
        //var basket = await _cache.GetOrCreateAsync(
        //    GetBasketKey(customerId),
        //    _ => ValueTask.FromResult(new CustomerBasket { CustomerId = customerId }),
        //    cancellationToken: cancellationToken);

        var basketString = await _cache.GetStringAsync(GetBasketKey(customerId), cancellationToken);
        var basket = string.IsNullOrEmpty(basketString)
            ? null
            : JsonSerializer.Deserialize(basketString, BasketSerializationContext.Default.CustomerBasket);


        return basket ?? new CustomerBasket { CustomerId = customerId };
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, CancellationToken cancellationToken = default)
    {
        //await _cache.SetAsync(GetBasketKey(basket.CustomerId), basket, cancellationToken: cancellationToken);

        var basketString = JsonSerializer.Serialize(basket, BasketSerializationContext.Default.CustomerBasket);
        await _cache.SetStringAsync(
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
