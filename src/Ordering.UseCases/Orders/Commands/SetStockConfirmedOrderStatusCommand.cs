namespace Ordering.UseCases.Orders.Commands;

public record SetStockConfirmedOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
