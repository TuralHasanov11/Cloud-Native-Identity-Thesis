namespace Basket.Core.BasketAggregate;

public class CustomerBasket
{
    public string CustomerId { get; set; }

    public List<BasketItem> Items { get; init; } = [];
}
