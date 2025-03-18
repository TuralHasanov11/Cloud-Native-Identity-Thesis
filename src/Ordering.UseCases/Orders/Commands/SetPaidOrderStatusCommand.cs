namespace Ordering.UseCases.Orders.Commands;

public sealed record SetPaidOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
