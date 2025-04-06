namespace Catalog.IntegrationTests.Brands;

public class BrandRepositoryTests : BaseIntegrationTest
{
    private readonly IBrandRepository _repository;
    private static readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(30));

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
        await _repository.CreateAsync(brand, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        // Assert
        var createdBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id));
        Assert.NotNull(createdBrand);
    }

    [Fact]
    public async Task Delete_ShouldRemoveBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand2");

        await _repository.CreateAsync(brand, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        // Act
        _repository.Delete(brand);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        // Assert
        var deletedBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id));
        Assert.Null(deletedBrand);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnBrands()
    {
        // Arrange
        var brand1 = Brand.Create("Brand3");
        var brand2 = Brand.Create("Brand4");

        await _repository.CreateAsync(brand1, _cancellationTokenSource.Token);
        await _repository.CreateAsync(brand2, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        var specification = new GetBrandsSpecification();

        // Act
        var brands = await _repository.ListAsync(specification, _cancellationTokenSource.Token);

        // Assert
        Assert.Contains(brands, b => b.Name == "Brand3");
        Assert.Contains(brands, b => b.Name == "Brand4");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand5");

        await _repository.CreateAsync(brand, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);
        var specification = new GetBrandSpecification(brand.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationTokenSource.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenBrandDoesNotExist()
    {
        // Arrange
        var specification = new GetBrandSpecification(new BrandId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationTokenSource.Token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand6");

        await _repository.CreateAsync(brand, _cancellationTokenSource.Token);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        // Act
        brand.UpdateName("UpdatedBrand");
        _repository.Update(brand);
        await _repository.SaveChangesAsync(_cancellationTokenSource.Token);

        // Assert
        var updatedBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id), _cancellationTokenSource.Token);
        Assert.NotNull(updatedBrand);
        Assert.Equal("UpdatedBrand", updatedBrand.Name);
    }
}
