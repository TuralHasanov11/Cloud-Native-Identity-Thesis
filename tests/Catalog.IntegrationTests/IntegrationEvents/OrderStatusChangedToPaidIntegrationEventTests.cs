namespace Catalog.IntegrationTests.IntegrationEvents;

//public class OrderStatusChangedToPaidIntegrationEventTests : BaseIntegrationTest
//{
//    private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

//    public OrderStatusChangedToPaidIntegrationEventTests(CatalogFactory factory)
//        : base(factory)
//    {
//        _logger = factory.Services.GetRequiredService<ILogger<OrderStatusChangedToPaidIntegrationEventHandler>>();
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Handle_ShouldRemoveStock_WhenStockIsAvailable()
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
//        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
//            Guid.NewGuid(),
//            new[] { orderStockItem });
//        var context = new ConsumeContextStub<OrderStatusChangedToPaidIntegrationEvent>(integrationEvent);

//        var handler = new OrderStatusChangedToPaidIntegrationEventHandler(
//            new ProductRepository(DbContext),
//            _logger);

//        // Act
//        await handler.Consume(context);

//        // Assert
//        var updatedProduct = await DbContext.Products.FindAsync(product.Id);
//        Assert.Equal(95, updatedProduct.AvailableStock);
//    }

//    [Fact(Skip = "Waiting")]
//    public async Task Handle_ShouldLogError_WhenProductNotFound()
//    {
//        // Arrange

//        var orderStockItem = new OrderStockItem(Guid.NewGuid(), 5);
//        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
//            Guid.NewGuid(),
//            new[] { orderStockItem });
//        var context = new ConsumeContextStub<OrderStatusChangedToPaidIntegrationEvent>(integrationEvent);

//        var handler = new OrderStatusChangedToPaidIntegrationEventHandler(
//            new ProductRepository(DbContext),
//            _logger);

//        // Act
//        await handler.Consume(context);

//        // Assert
//        // Verify that the logger logged the error (this would require checking the state of the _logger)
//    }
//}
