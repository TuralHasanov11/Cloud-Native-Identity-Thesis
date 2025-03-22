namespace Catalog.Contracts.Abstractions;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent message);

    Task PublishThroughEventBusAsync(IntegrationEvent message);
}
