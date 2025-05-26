using Catalog.Core.CatalogAggregate.Specifications;
using MassTransit;

namespace Catalog.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    IProductRepository productRepository,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    IConsumer<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToPaidIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        foreach (var orderStockItem in context.Message.OrderStockItems)
        {
            var product = await productRepository.SingleOrDefaultAsync(
                new GetProductSpecification(new ProductId(orderStockItem.ProductId)));

            if (product == null)
            {
                logger.LogProductNotFound(orderStockItem.ProductId);
                continue;
            }

            product.RemoveStock(orderStockItem.Units);
        }

        await productRepository.SaveChangesAsync();
    }
}


public static partial class OrderStatusChangedToPaidIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStatusChangedToPaidIntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Error, "Product with id {ProductId} not found.")]
    public static partial void LogProductNotFound(
        this ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger,
        Guid productId);
}
