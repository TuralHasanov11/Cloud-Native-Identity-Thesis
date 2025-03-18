namespace Ordering.UseCases.Orders.Commands;

public sealed record SetStockRejectedOrderStatusCommand(
    Guid OrderNumber,
    IEnumerable<Guid> OrderStockItems)
    : ICommand<bool>;
