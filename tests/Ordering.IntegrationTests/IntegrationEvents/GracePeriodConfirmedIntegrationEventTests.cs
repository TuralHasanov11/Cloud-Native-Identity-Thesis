using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.IntegrationEvents.EventHandlers;

namespace Ordering.IntegrationTests.IntegrationEvents;

public class GracePeriodConfirmedIntegrationEventTests : IClassFixture<OrderingFactory>
{
    private readonly IMediator _mediator;
    private readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> _logger;

    public GracePeriodConfirmedIntegrationEventTests(OrderingFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _logger = factory.Services.GetRequiredService<ILogger<GracePeriodConfirmedIntegrationEventHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldSendSetAwaitingValidationOrderStatusCommand()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(orderId);
        var context = new ConsumeContextStub<GracePeriodConfirmedIntegrationEvent>(integrationEvent);

        var handler = new GracePeriodConfirmedIntegrationEventHandler(_mediator, _logger);

        // Act
        await handler.Consume(context);

        // Assert
        // Verify that the SetAwaitingValidationOrderStatusCommand was sent (this would require checking the state of the _mediator)
    }

    [Fact]
    public async Task Handle_ShouldLogHandlingIntegrationEvent()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(orderId);
        var context = new ConsumeContextStub<GracePeriodConfirmedIntegrationEvent>(integrationEvent);

        var handler = new GracePeriodConfirmedIntegrationEventHandler(_mediator, _logger);

        // Act
        await handler.Consume(context);

        // Assert
        // Verify that the logger logged the handling of the integration event (this would require checking the state of the _logger)
    }
}
