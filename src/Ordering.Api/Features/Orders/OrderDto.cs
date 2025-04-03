namespace Ordering.Api.Features.Orders;

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

public sealed record OrderDraftDto(IEnumerable<OrderItemDto> OrderItems, decimal Total);

public record OrderSummary(Guid OrderNumber, DateTime Date, string Status, decimal Total);
