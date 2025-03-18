namespace Ordering.UseCases.Orders.Commands;

public record CancelOrderCommand(Guid OrderNumber) : ICommand<bool>;
