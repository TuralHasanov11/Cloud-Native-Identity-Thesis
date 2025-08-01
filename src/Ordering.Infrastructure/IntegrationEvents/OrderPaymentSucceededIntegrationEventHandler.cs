﻿using Ordering.Infrastructure.IntegrationEvents.Commands;

namespace Ordering.Infrastructure.IntegrationEvents.EventHandlers;

public sealed class OrderPaymentSucceededIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentSucceededIntegrationEventHandler> logger) :
    IConsumer<OrderPaymentSucceededIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPaymentSucceededIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        var command = new SetPaidOrderStatusCommand(context.Message.OrderId);

        await mediator.Send(command);
    }
}

public static partial class OrderPaymentSucceededIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderPaymentSucceededIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderPaymentSucceededIntegrationEvent integrationEvent);
}
