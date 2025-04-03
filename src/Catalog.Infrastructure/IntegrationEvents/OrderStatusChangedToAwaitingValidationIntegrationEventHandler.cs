using Catalog.Core.CatalogAggregate.Specifications;
using MassTransit;

namespace Catalog.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    IProductRepository productRepository,
    ICatalogIntegrationEventService catalogIntegrationEventService,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger) :
    IConsumer<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidationIntegrationEvent> context)
    {
        logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);
        var confirmedOrderStockItems = new List<ConfirmedOrderStockItemIntegrationEvent>();

        foreach (var orderStockItem in context.Message.OrderStockItems)
        {
            var product = await productRepository.SingleOrDefaultAsync(
                new GetProductByIdSpecification(new ProductId(orderStockItem.ProductId)));

            if (product == null)
            {
                logger.LogProductNotFound(orderStockItem.ProductId);
                return;
            }

            var hasStock = product.AvailableStock >= orderStockItem.Units;
            var confirmedOrderStockItem = new ConfirmedOrderStockItemIntegrationEvent(product.Id, hasStock);

            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }

        var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(context.Message.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(context.Message.OrderId);

        await catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
        await catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);
    }
}

public static partial class OrderStatusChangedToAwaitingValidationIntegrationEventHandlerLogger
{
    [LoggerMessage(LogLevel.Information, "Handling integration event: {IntegrationEventId} - ({IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(
        this ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger,
        Guid integrationEventId,
        OrderStatusChangedToAwaitingValidationIntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Error, "Product with id {ProductId} not found.")]
    public static partial void LogProductNotFound(
        this ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger,
        Guid productId);
}
