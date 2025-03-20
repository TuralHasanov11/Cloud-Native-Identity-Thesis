namespace Catalog.Contracts.IntegrationEvents;

public record ProductPriceChangedIntegrationEvent(
    Guid ProductId,
    decimal NewPrice,
    decimal OldPrice)
    : IntegrationEvent;
