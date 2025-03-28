namespace Ordering.Infrastructure.IntegrationEvents.Commands;


public sealed record SetStockRejectedOrderStatusCommand(
    Guid OrderNumber,
    IEnumerable<Guid> OrderStockItems)
    : ICommand<bool>;
