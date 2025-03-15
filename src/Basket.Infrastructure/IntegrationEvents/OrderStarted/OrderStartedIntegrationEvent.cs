using EventBus.Events;

namespace Basket.Infrastructure.IntegrationEvents.OrderStarted;

public record OrderStartedIntegrationEvent(Guid UserId) : IntegrationEvent;
