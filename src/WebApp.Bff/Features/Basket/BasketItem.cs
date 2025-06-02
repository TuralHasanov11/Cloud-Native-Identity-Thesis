namespace WebApp.Bff.Features.Basket;

public record BasketItem(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity)
{
    public static IEnumerable<BasketItem> Empty()
    {
        return [];
    }
}
