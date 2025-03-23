namespace Webhooks.Contracts.IntegrationEvents;

public record ProductPriceChangedIntegrationEvent(
    Guid ProductId,
    decimal NewPrice,
    decimal OldPrice)
    : IntegrationEvent;
