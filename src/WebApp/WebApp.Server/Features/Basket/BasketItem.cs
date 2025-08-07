namespace WebApp.Server.Features.Basket;

public record BasketItem(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    string? PictureUrl)
{
    public static IEnumerable<BasketItem> Empty()
    {
        return [];
    }
}
