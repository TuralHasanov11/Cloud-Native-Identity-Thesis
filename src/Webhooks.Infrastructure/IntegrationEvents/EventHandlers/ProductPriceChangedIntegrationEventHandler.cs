namespace Webhooks.Infrastructure.IntegrationEvents.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        return Task.CompletedTask;
    }
}
