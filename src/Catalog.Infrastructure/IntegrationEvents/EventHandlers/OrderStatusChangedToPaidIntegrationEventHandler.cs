using Catalog.Contracts.IntegrationEvents.Events;
using EventBus.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.IntegrationEvents.EventHandlers;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    CatalogDbContext catalogDbContext,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    private readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger = logger;

    public async Task Consume(ConsumeContext<OrderStatusChangedToPaidIntegrationEvent> context)
    {
        _logger.LogHandlingIntegrationEvent(context.Message.Id, context.Message);

        foreach (var orderStockItem in context.Message.OrderStockItems)
        {
            var product = await _catalogDbContext.Products.FindAsync(orderStockItem.ProductId);

            if (product == null)
            {
                _logger.LogProductNotFound(orderStockItem.ProductId);
                continue;
            }

            product.RemoveStock(orderStockItem.Units);
        }

        await _catalogDbContext.SaveChangesAsync();
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
