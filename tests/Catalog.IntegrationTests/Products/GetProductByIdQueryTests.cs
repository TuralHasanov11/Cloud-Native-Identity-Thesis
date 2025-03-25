using Catalog.UseCases.Products.GetById;

namespace Catalog.IntegrationTests.Products
{
    public class GetProductByIdQueryHandlerTests : IClassFixture<CatalogFactory>
    {
        private readonly IMediator _mediator;
        private readonly CatalogDbContext _dbContext;

        public GetProductByIdQueryHandlerTests(CatalogFactory factory)
        {
            _mediator = factory.Services.GetRequiredService<IMediator>();
            _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct()
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

            var query = new GetProductByIdQuery(product.Id.Value);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(product.Id.Value, result.Value.Id);
            Assert.Equal(product.Name, result.Value.Name);
            Assert.Equal(product.Description, result.Value.Description);
            Assert.Equal(product.Price, result.Value.Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var query = new GetProductByIdQuery(Guid.NewGuid());

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsNotFound());
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

            var query = new GetProductByIdQuery(product.Id.Value);

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
