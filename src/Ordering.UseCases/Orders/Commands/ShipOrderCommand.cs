namespace Ordering.UseCases.Orders.Commands;

public sealed record ShipOrderCommand(Guid OrderNumber) : ICommand<bool>;
