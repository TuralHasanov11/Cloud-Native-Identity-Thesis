using MassTransit;
using Outbox.Services;
using Outbox.Utilities;

namespace Catalog.Infrastructure.IntegrationEvents;

public sealed class CatalogIntegrationEventService(
    ILogger<CatalogIntegrationEventService> logger,
    IPublishEndpoint eventBus,
    CatalogDbContext catalogDbContext,
    IOutboxService outboxService)
    : ICatalogIntegrationEventService, IDisposable
{
    private volatile bool disposedValue;

    public async Task PublishThroughEventBusAsync(IntegrationEvent message)
    {
        try
        {
            logger.LogPublishingIntegrationEvent(message.Id, message);

            await outboxService.MarkEventAsInProgressAsync(message.Id);
            await eventBus.Publish(message);
            await outboxService.MarkEventAsPublishedAsync(message.Id);
        }
        catch (Exception ex)
        {
            logger.LogErrorPublishingIntegrationEvent(ex, message.Id, message);
            await outboxService.MarkEventAsFailedAsync(message.Id);
        }
    }

    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent message)
    {
        logger.LogSavingChangesAndIntegrationEvent(message.Id);

        await ResilientTransaction.New(catalogDbContext).ExecuteAsync(async () =>
        {
            await catalogDbContext.SaveChangesAsync();
            await outboxService.SaveEventAsync(message, catalogDbContext.Database.CurrentTransaction!);
        });
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                (outboxService as IDisposable)?.Dispose();
                catalogDbContext?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public static partial class CatalogIntegrationEventServiceLogger
{
    [LoggerMessage(LogLevel.Information, "Publishing integration event: {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogPublishingIntegrationEvent(
        this ILogger<CatalogIntegrationEventService> logger,
        Guid IntegrationEventId,
        IntegrationEvent IntegrationEvent);

    [LoggerMessage(LogLevel.Error, "Error Publishing integration event: {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogErrorPublishingIntegrationEvent(
        this ILogger<CatalogIntegrationEventService> logger,
        Exception exception,
        Guid IntegrationEventId,
        IntegrationEvent IntegrationEvent);

    [LoggerMessage(LogLevel.Information, "CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}")]
    public static partial void LogSavingChangesAndIntegrationEvent(
        this ILogger<CatalogIntegrationEventService> logger,
        Guid IntegrationEventId);
}
