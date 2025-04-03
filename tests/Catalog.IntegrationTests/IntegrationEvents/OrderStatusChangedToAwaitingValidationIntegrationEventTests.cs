namespace Catalog.IntegrationTests.IntegrationEvents;

//public class OrderStatusChangedToAwaitingValidationIntegrationEventTests
//    : BaseIntegrationTest
//{
//    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
//    private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

//    public OrderStatusChangedToAwaitingValidationIntegrationEventTests(CatalogFactory factory)
//        : base(factory)
//    {
//        _catalogIntegrationEventService = factory.Services.GetRequiredService<ICatalogIntegrationEventService>();
//        _logger = factory.Services.GetRequiredService<ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>>();
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Handle_ShouldConfirmOrderStock_WhenStockIsAvailable()
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
//        await DbContext.SaveChangesAsync();

//        var orderStockItem = new OrderStockItem(product.Id.Value, 5);
//        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
//        var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

//        var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
//            new ProductRepository(DbContext),
//            _catalogIntegrationEventService,
//            _logger);

//        // Act
//        await handler.Consume(context);

//        // Assert
//        var savedEvent = await DbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
//        Assert.NotNull(savedEvent);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Handle_ShouldRejectOrderStock_WhenStockIsNotAvailable()
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
//        await DbContext.SaveChangesAsync();

//        var orderStockItem = new OrderStockItem(product.Id.Value, 150);
//        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
//        var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

//        var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
//            new ProductRepository(DbContext),
//            _catalogIntegrationEventService,
//            _logger);

//        // Act
//        await handler.Consume(context);

//        // Assert
//        var savedEvent = await DbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
//        Assert.NotNull(savedEvent);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Handle_ShouldLogError_WhenProductNotFound()
//    {
//        // Arrange

//        var orderStockItem = new OrderStockItem(Guid.NewGuid(), 5);
//        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
//        var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

//        var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
//            new ProductRepository(DbContext),
//            _catalogIntegrationEventService,
//            _logger);

//        // Act
//        await handler.Consume(context);

//        // Assert
//        var savedEvent = await DbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
//        Assert.Null(savedEvent);
//    }
//}
