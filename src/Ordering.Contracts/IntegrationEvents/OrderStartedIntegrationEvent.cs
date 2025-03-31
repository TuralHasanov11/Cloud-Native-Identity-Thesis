namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStartedIntegrationEvent(string UserId) : IntegrationEvent;
