namespace Ordering.UseCases.Orders.Commands;

public sealed record CreateOrderDraftCommand(Guid CustomerId, IEnumerable<BasketItemDto> Items) : ICommand<OrderDraftDto>;
