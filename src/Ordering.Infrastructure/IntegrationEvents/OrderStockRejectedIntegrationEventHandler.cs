using Ordering.Infrastructure.IntegrationEvents.Commands;

namespace Ordering.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStockRejectedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger)
    : IConsumer<OrderStockRejectedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStockRejectedIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        var orderStockRejectedItems = context.Message.OrderStockItems
            .Where(c => !c.HasStock)
            .Select(c => c.ProductId);

        await mediator.Send(new SetStockRejectedOrderStatusCommand(context.Message.OrderId, orderStockRejectedItems));
    }
}

public static partial class OrderStockRejectedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStockRejectedIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStockRejectedIntegrationEvent integrationEvent);
}
