namespace Ordering.Infrastructure.Data.Queries;

public record OrderItemDto(string ProductName, int Units, decimal UnitPrice, Uri PictureUrl);

public record OrderDto(
    Guid OrderNumber,
    DateTime Date,
    string Description,
    string City,
    string Country,
    string State,
    string Street,
    string Zipcode,
    string Status,
    decimal Total,
    IEnumerable<OrderItemDto> OrderItems);

public record OrderSummary(Guid OrderNumber, DateTime Date, string Status, decimal Total);

public record CardTypeDto(int Id, string Name);

public static class OrderMapper
{
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
            [.. order.OrderItems.Select(oi => new OrderItemDto(oi.ProductName, oi.Units, oi.UnitPrice, oi.PictureUrl))]);
    }
}
