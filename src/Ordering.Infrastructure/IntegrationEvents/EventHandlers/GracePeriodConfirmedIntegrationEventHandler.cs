using MassTransit;
using Ordering.UseCases.Orders.Commands;

namespace Ordering.Infrastructure.IntegrationEvents.EventHandlers;

public sealed class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<GracePeriodConfirmedIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);
        var command = new SetAwaitingValidationOrderStatusCommand(context.Message.OrderId);

        await mediator.Send(command);
    }
}

public static partial class GracePeriodConfirmedIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<GracePeriodConfirmedIntegrationEventHandler> logger,
        Guid integrationEventId,
        GracePeriodConfirmedIntegrationEvent integrationEvent);
}
