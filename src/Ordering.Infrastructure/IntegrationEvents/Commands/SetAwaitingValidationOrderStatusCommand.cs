namespace Ordering.Infrastructure.IntegrationEvents.Commands;

public sealed record SetAwaitingValidationOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
