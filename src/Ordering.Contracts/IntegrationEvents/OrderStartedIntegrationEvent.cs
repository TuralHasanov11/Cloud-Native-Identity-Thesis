namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStartedIntegrationEvent(Guid UserId) : IntegrationEvent;
