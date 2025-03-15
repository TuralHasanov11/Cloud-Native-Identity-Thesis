using Basket.Core.BasketAggregate;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.IntegrationEvents.OrderStarted;

public sealed class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger) : IConsumer<OrderStartedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStartedIntegrationEvent> context)
    {
        var data = context.Message;

        logger.LogHandlingIntegrationEvent(data.Id, data);
        await repository.DeleteBasketAsync(data.UserId);
    }
}

public static partial class OrderStartedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStartedIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStartedIntegrationEvent integrationEvent);

}