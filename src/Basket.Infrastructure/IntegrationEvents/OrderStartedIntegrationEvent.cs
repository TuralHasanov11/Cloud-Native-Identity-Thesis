using EventBus.Events;

namespace Basket.Infrastructure.IntegrationEvents;

public record OrderStartedIntegrationEvent(Guid UserId) : IntegrationEvent;
