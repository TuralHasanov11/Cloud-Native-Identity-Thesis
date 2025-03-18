namespace Ordering.Contracts.IntegrationEvents.Events;

public sealed record OrderStartedIntegrationEvent(Guid UserId) : IntegrationEvent;
