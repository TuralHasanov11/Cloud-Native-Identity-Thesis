using Catalog.UseCases.ProductTypes.List;

namespace Catalog.IntegrationTests.ProductTypes
{
    public class ListProductTypesQueryHandlerTests : IClassFixture<CatalogFactory>
    {
        private readonly IMediator _mediator;
        private readonly CatalogDbContext _dbContext;

        public ListProductTypesQueryHandlerTests(CatalogFactory factory)
        {
            _mediator = factory.Services.GetRequiredService<IMediator>();
            _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
        }

        [Fact]
        public async Task Handle_ShouldReturnProductTypes()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var productType1 = ProductType.Create("Electronics");
            var productType2 = ProductType.Create("Clothing");

            _dbContext.ProductTypes.Add(productType1);
            _dbContext.ProductTypes.Add(productType2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductTypesQuery();

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains(result.Value, pt => pt.Name == "Electronics");
            Assert.Contains(result.Value, pt => pt.Name == "Clothing");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductTypesExist()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var query = new ListProductTypesQuery();

            // Act
            var result = await _mediator.Send(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowTaskCancelledException_WhenCancellationIsRequested()
        {
            // Arrange
            await _dbContext.SeedDatabase();

            var productType1 = ProductType.Create("Electronics");
            var productType2 = ProductType.Create("Clothing");

            _dbContext.ProductTypes.Add(productType1);
            _dbContext.ProductTypes.Add(productType2);
            await _dbContext.SaveChangesAsync();

            var query = new ListProductTypesQuery();

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
