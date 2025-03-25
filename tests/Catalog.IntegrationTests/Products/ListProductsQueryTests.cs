using Catalog.UseCases.Products.List;

namespace Catalog.IntegrationTests.Products
{
    public class ListProductsQueryHandlerTests : IClassFixture<CatalogFactory>
    {
        private readonly IMediator _mediator;
        private readonly CatalogDbContext _dbContext;

        public ListProductsQueryHandlerTests(CatalogFactory factory)
        {
            _mediator = factory.Services.GetRequiredService<IMediator>();
            _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedProducts()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var product1 = Product.Create(
                "Product1",
                "Description1",
                10.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                100,
                10,
                200);

            var product2 = Product.Create(
                "Product2",
                "Description2",
                20.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                200,
                20,
                400);

            _dbContext.Products.Add(product1);
            _dbContext.Products.Add(product2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductsQuery(Guid.Empty, 10, null, null, null);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            Assert.Contains(result.Value.Data, p => p.Name == "Product1");
            Assert.Contains(result.Value.Data, p => p.Name == "Product2");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var query = new ListProductsQuery(Guid.Empty, 10, null, null, null);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredProducts_ByName()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var product1 = Product.Create(
                "Product1",
                "Description1",
                10.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                100,
                10,
                200);

            var product2 = Product.Create(
                "Product2",
                "Description2",
                20.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                200,
                20,
                400);

            _dbContext.Products.Add(product1);
            _dbContext.Products.Add(product2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductsQuery(Guid.Empty, 10, "Product1", null, null);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.Data);
            Assert.Equal("Product1", result.Value.Data.First().Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredProducts_ByProductType()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var productTypeId = new ProductTypeId(Guid.NewGuid());

            var product1 = Product.Create(
                "Product1",
                "Description1",
                10.0m,
                productTypeId,
                new BrandId(Guid.NewGuid()),
                100,
                10,
                200);

            var product2 = Product.Create(
                "Product2",
                "Description2",
                20.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                200,
                20,
                400);

            _dbContext.Products.Add(product1);
            _dbContext.Products.Add(product2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductsQuery(Guid.Empty, 10, null, productTypeId.Value, null);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.Data);
            Assert.Equal("Product1", result.Value.Data.First().Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredProducts_ByBrand()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var brandId = new BrandId(Guid.NewGuid());

            var product1 = Product.Create(
                "Product1",
                "Description1",
                10.0m,
                new ProductTypeId(Guid.NewGuid()),
                brandId,
                100,
                10,
                200);

            var product2 = Product.Create(
                "Product2",
                "Description2",
                20.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                200,
                20,
                400);

            _dbContext.Products.Add(product1);
            _dbContext.Products.Add(product2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductsQuery(Guid.Empty, 10, null, null, brandId.Value);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.Data);
            Assert.Equal("Product1", result.Value.Data.First().Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowTaskCancelledException_WhenCancellationIsRequested()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var product1 = Product.Create(
                "Product1",
                "Description1",
                10.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                100,
                10,
                200);

            var product2 = Product.Create(
                "Product2",
                "Description2",
                20.0m,
                new ProductTypeId(Guid.NewGuid()),
                new BrandId(Guid.NewGuid()),
                200,
                20,
                400);

            _dbContext.Products.Add(product1);
            _dbContext.Products.Add(product2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductsQuery(Guid.Empty, 10, null, null, null);

            using var cancellationTokenSource = new CancellationTokenSource();
            await cancellationTokenSource.CancelAsync();

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await _mediator.Send(query, cancellationTokenSource.Token);
            });
        }
    }
}
