using Microsoft.Extensions.Logging;
using Webhooks.Api.Features.Webhooks;
using Webhooks.Infrastructure.IntegrationEvents;
using Webhooks.Infrastructure.Services;

namespace Webhooks.IntegrationTests.Webhooks.IntegrationEvents;

public class OrderStatusChangedToPaidIntegrationEventTests : IClassFixture<WebhooksFactory>
{
    private readonly WebhooksDbContext _dbContext;
    private readonly IWebhooksSender _webhooksSender;
    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

    public OrderStatusChangedToPaidIntegrationEventTests(WebhooksFactory factory)
    {
        _dbContext = factory.Services.GetRequiredService<WebhooksDbContext>();
        _logger = factory.Services.GetRequiredService<ILogger<OrderStatusChangedToPaidIntegrationEventHandler>>();
        _webhooksSender = new WebhooksSender(
            factory.Services.GetRequiredService<IHttpClientFactory>(),
            factory.Services.GetRequiredService<ILogger<WebhooksSender>>());
    }

    [Fact(Skip = "Waiting")]
    public async Task Handle_ShouldSendWebhooks_WhenSubscriptionsExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var subscription = new WebhookSubscription(
            WebhookType.OrderPaid,
            DateTime.UtcNow,
            new Uri("http://example.com/webhook"),
            "sample-token",
            new IdentityId(IdentityExtensions.GenerateId()));

        _dbContext.Subscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync();

        var orderStockItem = new OrderStockItem(Guid.NewGuid(), 5);
        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            Guid.NewGuid(),
            new[] { orderStockItem });
        var context = new ConsumeContextStub<OrderStatusChangedToPaidIntegrationEvent>(integrationEvent);

        var handler = new OrderStatusChangedToPaidIntegrationEventHandler(
            new WebhookSubscriptionRepository(_dbContext),
            _webhooksSender,
            _logger);

        // Act
        await handler.Consume(context);

        // Assert
    }

    [Fact(Skip = "Waiting")]
    public async Task Handle_ShouldLogInformation_WhenNoSubscriptionsExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var orderStockItem = new OrderStockItem(Guid.NewGuid(), 5);
        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            Guid.NewGuid(),
            new[] { orderStockItem });
        var context = new ConsumeContextStub<OrderStatusChangedToPaidIntegrationEvent>(integrationEvent);

        var handler = new OrderStatusChangedToPaidIntegrationEventHandler(
            new WebhookSubscriptionRepository(_dbContext),
            _webhooksSender,
            _logger);

        // Act
        await handler.Consume(context);

        // Assert
        // Verify that the logger logged the information (this would require checking the state of the _logger)
    }
}
