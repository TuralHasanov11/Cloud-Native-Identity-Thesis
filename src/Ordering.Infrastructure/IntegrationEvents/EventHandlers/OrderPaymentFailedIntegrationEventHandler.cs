using MassTransit;
using Ordering.Contracts.IntegrationEvents;
using Ordering.UseCases.Orders.Commands;

namespace Ordering.Infrastructure.IntegrationEvents.EventHandlers;

public sealed class OrderPaymentFailedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentFailedIntegrationEventHandler> logger) :
    IConsumer<OrderPaymentFailedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPaymentFailedIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        await mediator.Send(new CancelOrderCommand(context.Message.OrderId));
    }
}

public static partial class OrderPaymentFailedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderPaymentFailedIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderPaymentFailedIntegrationEvent integrationEvent);
}
