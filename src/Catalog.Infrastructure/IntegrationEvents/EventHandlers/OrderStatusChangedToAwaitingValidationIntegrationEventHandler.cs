using Catalog.Contracts.IntegrationEvents.Events;
using EventBus.Abstractions;
using EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    CatalogDbContext catalogDbContext,
    ICatalogIntegrationEventService catalogIntegrationEventService,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    private readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService = catalogIntegrationEventService;
    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger = logger;

    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidationIntegrationEvent> context)
    {
        _logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);
        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        foreach (var orderStockItem in context.Message.OrderStockItems)
        {
            var product = await _catalogDbContext.Products.FindAsync(orderStockItem.ProductId);

            if (product == null)
            {
                _logger.LogProductNotFound(orderStockItem.ProductId);
                return;
            }

            var hasStock = product.AvailableStock >= orderStockItem.Units;
            var confirmedOrderStockItem = new ConfirmedOrderStockItem(product.Id, hasStock);

            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }

        var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(context.Message.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(context.Message.OrderId);

        await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
        await _catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);
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
