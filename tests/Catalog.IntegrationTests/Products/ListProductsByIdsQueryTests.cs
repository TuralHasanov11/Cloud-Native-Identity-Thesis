using Catalog.UseCases.Products.ListByIds;

namespace Catalog.IntegrationTests.Products;

public class ListProductsByIdsQueryHandlerTests : IClassFixture<CatalogFactory>
{
    private readonly IMediator _mediator;
    private readonly CatalogDbContext _dbContext;

    public ListProductsByIdsQueryHandlerTests(CatalogFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsByIds()
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

        var query = new ListProductsByIdsQuery([product1.Id.Value, product2.Id.Value]);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
        Assert.Contains(result.Value, p => p.Name == "Product1");
        Assert.Contains(result.Value, p => p.Name == "Product2");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListProductsByIdsQuery([]);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenProductsNotFound()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListProductsByIdsQuery([Guid.NewGuid(), Guid.NewGuid()]);

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

        var query = new ListProductsByIdsQuery([product1.Id.Value, product2.Id.Value]);

        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await _mediator.Send(query, cancellationTokenSource.Token);
        });
    }
}
