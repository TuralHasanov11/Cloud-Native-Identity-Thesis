namespace Catalog.IntegrationTests.Brands;

public class BrandRepositoryTests : BaseIntegrationTest
{
    private readonly IBrandRepository _repository;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public BrandRepositoryTests(CatalogFactory factory)
        : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IBrandRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        // Act
        await _repository.CreateAsync(brand, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var createdBrand = await _repository.SingleOrDefaultAsync(
            new BrandSpecification(brand.Id),
            _cancellationToken);
        Assert.NotNull(createdBrand);
    }

    [Fact]
    public async Task Delete_ShouldRemoveBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand2");

        await _repository.CreateAsync(brand, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        _repository.Delete(brand);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var deletedBrand = await _repository.SingleOrDefaultAsync(
            new BrandSpecification(brand.Id),
            _cancellationToken);
        Assert.Null(deletedBrand);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnBrands()
    {
        // Arrange
        var brand1 = Brand.Create("Brand3");
        var brand2 = Brand.Create("Brand4");

        await _repository.CreateAsync(brand1, _cancellationToken);
        await _repository.CreateAsync(brand2, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        var specification = new BrandSpecification();

        // Act
        var brands = await _repository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.Contains(brands, b => b.Name == "Brand3");
        Assert.Contains(brands, b => b.Name == "Brand4");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand5");

        await _repository.CreateAsync(brand, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);
        var specification = new BrandSpecification(brand.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenBrandDoesNotExist()
    {
        // Arrange
        var specification = new BrandSpecification(new BrandId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand6");

        await _repository.CreateAsync(brand, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        brand.UpdateName("UpdatedBrand");
        _repository.Update(brand);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var updatedBrand = await _repository.SingleOrDefaultAsync(new BrandSpecification(brand.Id), _cancellationToken);
        Assert.NotNull(updatedBrand);
        Assert.Equal("UpdatedBrand", updatedBrand.Name);
    }
}
