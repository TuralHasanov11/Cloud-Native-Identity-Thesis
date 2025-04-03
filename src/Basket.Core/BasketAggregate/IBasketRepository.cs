namespace Basket.Core.BasketAggregate;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    Task<CustomerBasket> UpdateBasketAsync(
        CustomerBasket basket,
        CancellationToken cancellationToken = default);

    Task DeleteBasketAsync(
        string customerId,
        CancellationToken cancellationToken = default);
}
