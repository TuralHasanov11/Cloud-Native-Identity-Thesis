namespace Basket.Core.BasketAggregate;

public class CustomerBasket
{
    public Guid CustomerId { get; set; }

    public List<BasketItem> Items { get; init; } = [];
}
