namespace Basket.Core.BasketAggregate;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<CustomerBasket> UpdateBasketAsync(
        CustomerBasket basket,
        CancellationToken cancellationToken = default);

    Task DeleteBasketAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
