using Catalog.Infrastructure.IntegrationEvents;
using EventBus.Events;
using Outbox.Services;

namespace Catalog.IntegrationTests.IntegrationEvents;

public class CatalogIntegrationEventServiceTests : IClassFixture<CatalogFactory>
{
    private readonly CatalogDbContext _dbContext;
    private readonly IPublishEndpoint _eventBus;
    private readonly IOutboxService _outboxService;
    private readonly ILogger<CatalogIntegrationEventService> _logger;

    public CatalogIntegrationEventServiceTests(CatalogFactory factory)
    {
        _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        _eventBus = factory.Services.GetRequiredService<IPublishEndpoint>();
        _outboxService = factory.Services.GetRequiredService<IOutboxService>();
        _logger = factory.Services.GetRequiredService<ILogger<CatalogIntegrationEventService>>();
    }

    [Fact(Skip = "Waiting")]
    public async Task PublishThroughEventBusAsync_ShouldPublishEvent()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var integrationEvent = new IntegrationEvent();
        using var service = new CatalogIntegrationEventService(_logger, _eventBus, _dbContext, _outboxService);

        // Act
        await service.PublishThroughEventBusAsync(integrationEvent);

        // Assert
        var publishedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
        Assert.NotNull(publishedEvent);
        Assert.Equal(EventState.Published, publishedEvent.State);
    }

    [Fact(Skip = "Waiting")]
    public async Task PublishThroughEventBusAsync_ShouldMarkEventAsFailed_WhenExceptionIsThrown()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var integrationEvent = new IntegrationEvent();
        using var service = new CatalogIntegrationEventService(_logger, _eventBus, _dbContext, _outboxService);

        // Simulate an exception by disposing the event bus
        (_eventBus as IDisposable)?.Dispose();

        // Act
        await service.PublishThroughEventBusAsync(integrationEvent);

        // Assert
        var failedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
        Assert.NotNull(failedEvent);
        Assert.Equal(EventState.PublishedFailed, failedEvent.State);
    }

    [Fact(Skip = "Waiting")]
    public async Task SaveEventAndCatalogContextChangesAsync_ShouldSaveEventAndContextChanges()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        _dbContext.Products.Add(product);

        var integrationEvent = new IntegrationEvent();
        using var service = new CatalogIntegrationEventService(_logger, _eventBus, _dbContext, _outboxService);

        // Act
        await service.SaveEventAndCatalogContextChangesAsync(integrationEvent);

        // Assert
        var savedProduct = await _dbContext.Products.FindAsync(product.Id);
        Assert.NotNull(savedProduct);

        var savedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
        Assert.NotNull(savedEvent);
    }
}
