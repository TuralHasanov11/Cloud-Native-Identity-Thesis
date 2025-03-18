namespace Ordering.UseCases.Orders;

public static class OrderExtensions
{
    public static OrderItemDto ToOrderItemDto(this BasketItemDto item)
    {
        return new OrderItemDto(item.ProductId, item.ProductName, item.PictureUrl, item.UnitPrice, item.Quantity);
    }

    public static OrderDraftDto ToOrderDraftDto(this Order order)
    {
        return new OrderDraftDto(
            order.OrderItems.Select(oi => new OrderItemDto(
                ProductId: oi.ProductId,
                ProductName: oi.ProductName,
                PictureUrl: oi.PictureUrl,
                UnitPrice: oi.UnitPrice,
                Units: oi.Units,
                Discount: oi.Discount)),
            order.GetTotal());
    }
}
