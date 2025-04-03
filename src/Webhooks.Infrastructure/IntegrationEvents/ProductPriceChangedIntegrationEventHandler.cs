namespace Webhooks.Infrastructure.IntegrationEvents;

public class ProductPriceChangedIntegrationEventHandler : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        return Task.CompletedTask;
    }
}
