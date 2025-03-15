namespace Basket.Core.BasketAggregate;

public class CustomerBasket
{
    public Guid CustomerId { get; set; }

    public List<BasketItem> Items { get; } = [];

    public CustomerBasket() { }

    public CustomerBasket(Guid customerId)
    {
        CustomerId = customerId;
    }
}
