namespace Ordering.UseCases.Orders;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderItemDto> ToOrderItemsDTO(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDto();
        }
    }

    public static OrderItemDto ToOrderItemDto(this BasketItem item)
    {
        return new OrderItemDto(item.ProductId, item.ProductName, item.PictureUrl, item.UnitPrice, item.Quantity);
    }
}
