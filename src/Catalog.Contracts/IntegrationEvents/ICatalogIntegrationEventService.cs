using EventBus.Events;

namespace Catalog.Contracts.IntegrationEvents;

public interface ICatalogIntegrationEventService
{
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent message);

    Task PublishThroughEventBusAsync(IntegrationEvent message);
}
