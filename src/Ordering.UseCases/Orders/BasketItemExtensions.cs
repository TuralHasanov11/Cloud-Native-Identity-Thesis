namespace Ordering.UseCases.Orders;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderItemDto> ToOrderItemsDto(this IEnumerable<BasketItemDto> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDto();
        }
    }
}
