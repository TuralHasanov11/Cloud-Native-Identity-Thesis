namespace Catalog.IntegrationTests.Brands;

public class BrandRepositoryTests : BaseIntegrationTest
{
    private readonly IBrandRepository _repository;

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
        await _repository.CreateAsync(brand);
        await _repository.SaveChangesAsync();

        // Assert
        var createdBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id));
        Assert.NotNull(createdBrand);
    }

    [Fact]
    public async Task Delete_ShouldRemoveBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        await _repository.CreateAsync(brand);
        await _repository.SaveChangesAsync();

        // Act
        _repository.Delete(brand);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id));
        Assert.Null(deletedBrand);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnBrands()
    {
        // Arrange
        var brand1 = Brand.Create("Brand1");
        var brand2 = Brand.Create("Brand2");

        await _repository.CreateAsync(brand1);
        await _repository.CreateAsync(brand2);
        await _repository.SaveChangesAsync();

        var specification = new GetBrandsSpecification();

        // Act
        var brands = await _repository.ListAsync(specification);

        // Assert
        Assert.Contains(brands, b => b.Name == "Brand1");
        Assert.Contains(brands, b => b.Name == "Brand2");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        await _repository.CreateAsync(brand);
        await _repository.SaveChangesAsync();
        var specification = new GetBrandSpecification(brand.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

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
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyBrand()
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        await _repository.CreateAsync(brand);
        await _repository.SaveChangesAsync();

        // Act
        brand.UpdateName("UpdatedBrand");
        _repository.Update(brand);
        await _repository.SaveChangesAsync();

        // Assert
        var updatedBrand = await _repository.SingleOrDefaultAsync(new GetBrandSpecification(brand.Id));
        Assert.NotNull(updatedBrand);
        Assert.Equal("UpdatedBrand", updatedBrand.Name);
    }
}
