namespace Ordering.Contracts.IntegrationEvents;

public sealed record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string CustomerName,
    string CustomerIdentityId) : IntegrationEvent;
