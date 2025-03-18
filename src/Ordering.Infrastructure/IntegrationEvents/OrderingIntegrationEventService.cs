using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;
using Outbox.Services;

namespace Ordering.Infrastructure.IntegrationEvents;

public class OrderingIntegrationEventService(IEventBus eventBus,
    OrderingDbContext orderingContext,
    IOutboxService outboxService,
    ILogger<OrderingIntegrationEventService> logger) : IOrderingIntegrationEventService
{
    private readonly IEventBus _eventBus = eventBus;
    private readonly OrderingDbContext _orderingContext = orderingContext;
    private readonly IOutboxService _outboxService = outboxService;
    private readonly ILogger<OrderingIntegrationEventService> _logger = logger;

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await _outboxService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogPublishingIntegrationEvent(logEvt.Id, logEvt.IntegrationEvent);

            try
            {
                await _outboxService.MarkEventAsInProgressAsync(logEvt.Id);
                await _eventBus.Publish(logEvt.IntegrationEvent);
                await _outboxService.MarkEventAsPublishedAsync(logEvt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogErrorPublishingIntegrationEvent(ex, logEvt.Id);

                await _outboxService.MarkEventAsFailedAsync(logEvt.Id);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        _logger.LogEnqueuingIntegrationEventToRepository(evt.Id, evt);

        await _outboxService.SaveEventAsync(evt, _orderingContext.GetCurrentTransaction());
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
