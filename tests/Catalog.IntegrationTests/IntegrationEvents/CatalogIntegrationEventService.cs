namespace Catalog.IntegrationTests.IntegrationEvents;

//public class CatalogIntegrationEventServiceTests : BaseIntegrationTest
//{
//    private readonly IPublishEndpoint _eventBus;
//    private readonly IOutboxService _outboxService;
//    private readonly ILogger<CatalogIntegrationEventService> _logger;

//    public CatalogIntegrationEventServiceTests(CatalogFactory factory) : base(factory)
//    {
//        _eventBus = factory.Services.GetRequiredService<IPublishEndpoint>();
//        _outboxService = factory.Services.GetRequiredService<IOutboxService>();
//        _logger = factory.Services.GetRequiredService<ILogger<CatalogIntegrationEventService>>();
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task PublishThroughEventBusAsync_ShouldPublishEvent()
//    {
//        // Arrange
//        var integrationEvent = new IntegrationEvent();
//        using var service = new CatalogIntegrationEventService(_logger, _eventBus, DbContext, _outboxService);

//        // Act
//        await service.PublishThroughEventBusAsync(integrationEvent);

//        // Assert
//        var publishedEvent = await DbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
//        Assert.NotNull(publishedEvent);
//        Assert.Equal(EventState.Published, publishedEvent.State);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task PublishThroughEventBusAsync_ShouldMarkEventAsFailed_WhenExceptionIsThrown()
//    {
//        // Arrange

//        var integrationEvent = new IntegrationEvent();
//        using var service = new CatalogIntegrationEventService(_logger, _eventBus, DbContext, _outboxService);

//        // Simulate an exception by disposing the event bus
//        (_eventBus as IDisposable)?.Dispose();

//        // Act
//        await service.PublishThroughEventBusAsync(integrationEvent);

//        // Assert
//        var failedEvent = await DbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
//        Assert.NotNull(failedEvent);
//        Assert.Equal(EventState.PublishedFailed, failedEvent.State);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task SaveEventAndCatalogContextChangesAsync_ShouldSaveEventAndContextChanges()
//    {
//        // Arrange

//        var product = Product.Create(
//            "Product1",
//            "Description1",
//            10.0m,
//            new ProductTypeId(Guid.NewGuid()),
//            new BrandId(Guid.NewGuid()),
//            100,
//            10,
//            200);

//        DbContext.Products.Add(product);

//        var integrationEvent = new IntegrationEvent();
//        using var service = new CatalogIntegrationEventService(_logger, _eventBus, DbContext, _outboxService);

//        // Act
//        await service.SaveEventAndCatalogContextChangesAsync(integrationEvent);

//        // Assert
//        var savedProduct = await DbContext.Products.FindAsync(product.Id);
//        Assert.NotNull(savedProduct);

//        var savedEvent = await DbContext.Set<OutboxMessage>().FindAsync(integrationEvent.Id);
//        Assert.NotNull(savedEvent);
//    }
//}
