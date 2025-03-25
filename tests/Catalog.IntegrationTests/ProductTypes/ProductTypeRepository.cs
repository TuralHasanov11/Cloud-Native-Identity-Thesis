namespace Catalog.IntegrationTests.ProductTypes;

public class ProductTypeRepositoryTests : IClassFixture<CatalogFactory>
{
    private readonly CatalogFactory _factory;

    public ProductTypeRepositoryTests(CatalogFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProductType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);
        var productType = ProductType.Create("Electronics");

        // Act
        await repository.CreateAsync(productType);
        await repository.SaveChangesAsync();

        // Assert
        var createdProductType = await dbContext.ProductTypes.FindAsync(productType.Id);
        Assert.NotNull(createdProductType);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProductType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);
        var productType = ProductType.Create("Electronics");

        await repository.CreateAsync(productType);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(productType);
        await repository.SaveChangesAsync();

        // Assert
        var deletedProductType = await dbContext.ProductTypes.FindAsync(productType.Id);
        Assert.Null(deletedProductType);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnProductTypes()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);

        var productType1 = ProductType.Create("Electronics");
        var productType2 = ProductType.Create("Clothing");

        await repository.CreateAsync(productType1);
        await repository.CreateAsync(productType2);
        await repository.SaveChangesAsync();

        var specification = new GetProductTypesSpecification();

        // Act
        var productTypes = await repository.ListAsync(specification);

        // Assert
        Assert.Contains(productTypes, pt => pt.Name == "Electronics");
        Assert.Contains(productTypes, pt => pt.Name == "Clothing");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProductType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);
        var productType = ProductType.Create("Electronics");

        await repository.CreateAsync(productType);
        await repository.SaveChangesAsync();
        var specification = new GetProductTypeSpecification(productType.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productType.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductTypeDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);
        var specification = new GetProductTypeSpecification(new ProductTypeId(Guid.NewGuid()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProductType()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductTypeRepository(dbContext);
        var productType = ProductType.Create("Electronics");

        await repository.CreateAsync(productType);
        await repository.SaveChangesAsync();

        // Act
        productType.UpdateName("UpdatedElectronics");
        repository.Update(productType);
        await repository.SaveChangesAsync();

        // Assert
        var updatedProductType = await dbContext.ProductTypes.FindAsync(productType.Id);
        Assert.NotNull(updatedProductType);
        Assert.Equal("UpdatedElectronics", updatedProductType.Name);
    }
}
