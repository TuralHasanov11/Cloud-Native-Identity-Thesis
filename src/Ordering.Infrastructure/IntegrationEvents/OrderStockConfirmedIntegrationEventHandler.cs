using MassTransit;
using Ordering.Contracts.IntegrationEvents;
using Ordering.UseCases.Orders.Commands;

namespace Ordering.Infrastructure.IntegrationEvents.EventHandlers;

public sealed class OrderStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
    : IConsumer<OrderStockConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStockConfirmedIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        await mediator.Send(new SetStockConfirmedOrderStatusCommand(context.Message.OrderId));
    }
}

public static partial class OrderStockConfirmedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStockConfirmedIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStockConfirmedIntegrationEvent integrationEvent);
}
