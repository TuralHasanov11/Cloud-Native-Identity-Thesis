using Catalog.Infrastructure.IntegrationEvents.EventHandlers;

namespace Catalog.IntegrationTests.IntegrationEvents
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEventTests : IClassFixture<CatalogFactory>
    {
        private readonly CatalogDbContext _dbContext;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
        private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public OrderStatusChangedToAwaitingValidationIntegrationEventTests(CatalogFactory factory)
        {
            _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
            _catalogIntegrationEventService = factory.Services.GetRequiredService<ICatalogIntegrationEventService>();
            _logger = factory.Services.GetRequiredService<ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldConfirmOrderStock_WhenStockIsAvailable()
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
            await _dbContext.SaveChangesAsync();

            var orderStockItem = new OrderStockItem(product.Id.Value, 5);
            var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
            var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

            var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
                new ProductRepository(_dbContext),
                _catalogIntegrationEventService,
                _logger);

            // Act
            await handler.Consume(context);

            // Assert
            var savedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
            Assert.NotNull(savedEvent);
        }

        [Fact]
        public async Task Handle_ShouldRejectOrderStock_WhenStockIsNotAvailable()
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
            await _dbContext.SaveChangesAsync();

            var orderStockItem = new OrderStockItem(product.Id.Value, 150);
            var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
            var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

            var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
                new ProductRepository(_dbContext),
                _catalogIntegrationEventService,
                _logger);

            // Act
            await handler.Consume(context);

            // Assert
            var savedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
            Assert.NotNull(savedEvent);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenProductNotFound()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var orderStockItem = new OrderStockItem(Guid.NewGuid(), 5);
            var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid.NewGuid(), new[] { orderStockItem });
            var context = new ConsumeContextStub<OrderStatusChangedToAwaitingValidationIntegrationEvent>(integrationEvent);

            var handler = new OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
                new ProductRepository(_dbContext),
                _catalogIntegrationEventService,
                _logger);

            // Act
            await handler.Consume(context);

            // Assert
            var savedEvent = await _dbContext.Set<OutboxMessage>().FindAsync(context.Message.Id);
            Assert.Null(savedEvent);
        }
    }


}
