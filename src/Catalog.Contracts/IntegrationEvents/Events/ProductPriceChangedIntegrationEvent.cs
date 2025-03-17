namespace Catalog.Contracts.IntegrationEvents.Events;

public record ProductPriceChangedIntegrationEvent(
    Guid ProductId,
    decimal NewPrice,
    decimal OldPrice)
    : IntegrationEvent;
