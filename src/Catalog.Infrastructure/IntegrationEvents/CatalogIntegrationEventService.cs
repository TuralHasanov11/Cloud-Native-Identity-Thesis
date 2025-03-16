using EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<CatalogIntegrationEventService> _logger = logger;
    private readonly IPublishEndpoint _eventBus = eventBus;
    private readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    private readonly IOutboxService _outboxService = outboxService;

    public async Task PublishThroughEventBusAsync(IntegrationEvent message)
    {
        try
        {
            _logger.LogPublishingIntegrationEvent(message.Id, message);

            await _outboxService.MarkEventAsInProgressAsync(message.Id);
            await _eventBus.Publish(message);
            await _outboxService.MarkEventAsPublishedAsync(message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogErrorPublishingIntegrationEvent(ex, message.Id, message);
            await _outboxService.MarkEventAsFailedAsync(message.Id);
        }
    }

    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent message)
    {
        _logger.LogSavingChangesAndIntegrationEvent(message.Id);

        await ResilientTransaction.New(_catalogDbContext).ExecuteAsync(async () =>
        {
            await _catalogDbContext.SaveChangesAsync();
            await _outboxService.SaveEventAsync(message, _catalogDbContext.Database.CurrentTransaction!);
        });
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                (_outboxService as IDisposable)?.Dispose();
                _catalogDbContext?.Dispose();
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
