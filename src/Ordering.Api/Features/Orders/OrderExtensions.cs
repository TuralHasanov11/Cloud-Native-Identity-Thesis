namespace Ordering.Api.Features.Orders;

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

    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.OrderDate,
            order.Description,
            order.Address.City,
            order.Address.Country,
            order.Address.State,
            order.Address.Street,
            order.Address.ZipCode,
            order.OrderStatus.ToString(),
            order.GetTotal(),
            [.. order.OrderItems.Select(oi => new OrderItemDto(oi.ProductId, oi.ProductName, oi.PictureUrl, oi.UnitPrice, oi.Units))]);
    }

    public static OrderSummary ToOrderSummary(this Order order)
    {
        return new OrderSummary(
            order.Id,
            order.OrderDate,
            order.OrderStatus.ToString(),
            order.OrderItems.Sum(oi => oi.UnitPrice * oi.Units));
    }
}
