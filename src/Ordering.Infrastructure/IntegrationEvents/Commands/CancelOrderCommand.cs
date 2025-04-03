namespace Ordering.Infrastructure.IntegrationEvents.Commands;

public record CancelOrderCommand(Guid OrderNumber) : ICommand<bool>;
