namespace Catalog.IntegrationTests.Brands;

public class BrandRepositoryTests : IClassFixture<CatalogFactory>
{
    private readonly CatalogFactory _factory;

    public BrandRepositoryTests(CatalogFactory factory)
    {
        _factory = factory;
    }

    [Fact(Skip = "Waiting")]
    public async Task CreateAsync_ShouldAddBrand()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);
        var brand = Brand.Create("Brand1");

        // Act
        await repository.CreateAsync(brand);
        await repository.SaveChangesAsync();

        // Assert
        var createdBrand = await dbContext.Brands.FindAsync(brand.Id);
        Assert.NotNull(createdBrand);
    }

    [Fact(Skip = "Waiting")]
    public async Task Delete_ShouldRemoveBrand()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);
        var brand = Brand.Create("Brand1");

        await repository.CreateAsync(brand);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(brand);
        await repository.SaveChangesAsync();

        // Assert
        var deletedBrand = await dbContext.Brands.FindAsync(brand.Id);
        Assert.Null(deletedBrand);
    }

    [Fact(Skip = "Waiting")]
    public async Task ListAsync_ShouldReturnBrands()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);

        var brand1 = Brand.Create("Brand1");
        var brand2 = Brand.Create("Brand2");

        await repository.CreateAsync(brand1);
        await repository.CreateAsync(brand2);
        await repository.SaveChangesAsync();

        var specification = new GetBrandsSpecification();

        // Act
        var brands = await repository.ListAsync(specification);

        // Assert
        Assert.Contains(brands, b => b.Name == "Brand1");
        Assert.Contains(brands, b => b.Name == "Brand2");
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnBrand()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);
        var brand = Brand.Create("Brand1");

        await repository.CreateAsync(brand);
        await repository.SaveChangesAsync();
        var specification = new GetBrandSpecification(brand.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.Id);
    }

    [Fact(Skip = "Waiting")]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenBrandDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);
        var specification = new GetBrandSpecification(new BrandId(Guid.NewGuid()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact(Skip = "Waiting")]
    public async Task Update_ShouldModifyBrand()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new BrandRepository(dbContext);
        var brand = Brand.Create("Brand1");

        await repository.CreateAsync(brand);
        await repository.SaveChangesAsync();

        // Act
        brand.UpdateName("UpdatedBrand");
        repository.Update(brand);
        await repository.SaveChangesAsync();

        // Assert
        var updatedBrand = await dbContext.Brands.FindAsync(brand.Id);
        Assert.NotNull(updatedBrand);
        Assert.Equal("UpdatedBrand", updatedBrand.Name);
    }
}
