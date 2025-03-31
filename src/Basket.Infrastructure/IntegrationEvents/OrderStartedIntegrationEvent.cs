using EventBus.Events;

namespace Basket.Infrastructure.IntegrationEvents;

public sealed record OrderStartedIntegrationEvent(string UserId) : IntegrationEvent;
