namespace Ordering.Infrastructure.IntegrationEvents.Commands;


public record SetStockConfirmedOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
