using EventBus.Events;
using MassTransit;
using Ordering.Contracts.Abstractions;
using Outbox.Services;

namespace Ordering.Infrastructure.IntegrationEvents;

public class OrderingIntegrationEventService(
    IPublishEndpoint eventBus,
    OrderingDbContext orderingContext,
    IOutboxService outboxService,
    ILogger<OrderingIntegrationEventService> logger)
    : IOrderingIntegrationEventService
{
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await outboxService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            logger.LogPublishingIntegrationEvent(logEvt.Id, logEvt.IntegrationEvent);

            try
            {
                await outboxService.MarkEventAsInProgressAsync(logEvt.Id);
                await eventBus.Publish(logEvt.IntegrationEvent);
                await outboxService.MarkEventAsPublishedAsync(logEvt.Id);
            }
            catch (Exception ex)
            {
                logger.LogErrorPublishingIntegrationEvent(ex, logEvt.Id);

                await outboxService.MarkEventAsFailedAsync(logEvt.Id);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        logger.LogEnqueuingIntegrationEventToRepository(evt.Id, evt);

        await outboxService.SaveEventAsync(evt, orderingContext.GetCurrentTransaction());
    }
}


public static partial class OrderingIntegrationEventServiceLogger
{
    [LoggerMessage(LogLevel.Information, "Enqueuing integration event {IntegrationEventId} to repository ({IntegrationEvent})")]
    public static partial void LogEnqueuingIntegrationEventToRepository(
        this ILogger<OrderingIntegrationEventService> logger,
        Guid integrationEventId,
        IntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Information, "Publishing integration event {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogPublishingIntegrationEvent(
        this ILogger<OrderingIntegrationEventService> logger,
        Guid integrationEventId,
        IntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Error, "Error publishing integration event: {IntegrationEventId}")]
    public static partial void LogErrorPublishingIntegrationEvent(
        this ILogger<OrderingIntegrationEventService> logger,
        Exception exception,
        Guid integrationEventId);
}
