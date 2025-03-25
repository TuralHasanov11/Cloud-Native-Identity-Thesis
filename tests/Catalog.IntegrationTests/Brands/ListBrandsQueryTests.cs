using Catalog.UseCases.Brands.List;

namespace Catalog.IntegrationTests.Brands;

public class ListBrandsQueryTests : IClassFixture<CatalogFactory>
{
    private readonly IMediator _mediator;
    private readonly CatalogDbContext _dbContext;

    public ListBrandsQueryTests(CatalogFactory factory)
    {
        _mediator = factory.Services.GetRequiredService<IMediator>();
        _dbContext = factory.Services.GetRequiredService<CatalogDbContext>();
    }

    [Fact]
    public async Task Handle_ShouldReturnBrands()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListBrandsQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value, b => b.Name == CatalogDbContextExtensions.Brand1.Name);
        Assert.Contains(result.Value, b => b.Name == CatalogDbContextExtensions.Brand2.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBrandsExist()
    {
        // Arrange
        await _dbContext.SeedDatabase();

        var query = new ListBrandsQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}
