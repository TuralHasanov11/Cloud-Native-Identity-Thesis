namespace Ordering.Infrastructure.IntegrationEvents.Commands;


public sealed record SetPaidOrderStatusCommand(Guid OrderNumber) : ICommand<bool>;
