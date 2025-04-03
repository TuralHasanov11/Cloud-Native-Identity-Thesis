namespace Catalog.Contracts.IntegrationEvents;

public sealed record ProductPriceChangedIntegrationEvent(
    Guid ProductId,
    decimal NewPrice,
    decimal OldPrice)
    : IntegrationEvent;
