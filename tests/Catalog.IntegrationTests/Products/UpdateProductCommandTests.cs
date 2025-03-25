using Catalog.UseCases.Products.Update;

namespace Catalog.IntegrationTests.Products
{
    public class UpdateProductCommandHandlerTests : IClassFixture<CatalogFactory>
    {
        private readonly IMediator _mediator;
        private readonly CatalogDbContext _dbContext;

        public UpdateProductCommandHandlerTests(CatalogFactory factory)
        {
            _mediator = factory.Services.GetRequiredService<IMediator>();
            _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct()
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

            var command = new UpdateProductCommand(
                product.Id.Value,
                "UpdatedProduct",
                "UpdatedDescription",
                15.0m,
                product.ProductTypeId.Value,
                product.BrandId.Value,
                150,
                15,
                300);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.True(result.IsSuccess);
            var updatedProduct = await _dbContext.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal(command.Name, updatedProduct.Name);
            Assert.Equal(command.Description, updatedProduct.Description);
            Assert.Equal(command.Price, updatedProduct.Price);
            Assert.Equal(command.AvailableStock, updatedProduct.AvailableStock);
            Assert.Equal(command.RestockThreshold, updatedProduct.RestockThreshold);
            Assert.Equal(command.MaxStockThreshold, updatedProduct.MaxStockThreshold);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "UpdatedProduct",
                "UpdatedDescription",
                15.0m,
                Guid.NewGuid(),
                Guid.NewGuid(),
                150,
                15,
                300);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsNotFound());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductNameIsEmpty()
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

            var command = new UpdateProductCommand(
                product.Id.Value,
                string.Empty,
                "UpdatedDescription",
                15.0m,
                product.ProductTypeId.Value,
                product.BrandId.Value,
                150,
                15,
                300);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.ValidationErrors);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenProductPriceIsNegative()
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

            var command = new UpdateProductCommand(
                product.Id.Value,
                "UpdatedProduct",
                "UpdatedDescription",
                -15.0m,
                product.ProductTypeId.Value,
                product.BrandId.Value,
                150,
                15,
                300);

            // Act
            var result = await _mediator.Send(command);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.ValidationErrors);
        }

        [Fact]
        public async Task Handle_ShouldThrowTaskCancelledException_WhenCancellationIsRequested()
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

            var command = new UpdateProductCommand(
                product.Id.Value,
                "UpdatedProduct",
                "UpdatedDescription",
                15.0m,
                product.ProductTypeId.Value,
                product.BrandId.Value,
                150,
                15,
                300);

            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await _mediator.Send(command, cancellationTokenSource.Token);
            });
        }
    }
}
