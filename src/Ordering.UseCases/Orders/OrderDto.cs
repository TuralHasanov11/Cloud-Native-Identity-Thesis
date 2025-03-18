namespace Ordering.UseCases.Orders;

public sealed record OrderDraftDto(IEnumerable<OrderItemDto> OrderItems, decimal Total);
