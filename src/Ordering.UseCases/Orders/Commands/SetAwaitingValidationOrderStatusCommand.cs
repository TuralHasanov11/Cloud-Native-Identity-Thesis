namespace Ordering.UseCases.Orders.Commands;

public sealed record SetAwaitingValidationOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
