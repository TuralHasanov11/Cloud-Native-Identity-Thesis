namespace Catalog.IntegrationTests.ProductTypes;

[Collection(nameof(IntegrationTestCollection))]
public class ProductTypeRepositoryTests : BaseIntegrationTest
{
    private readonly IProductTypeRepository _repository;

    public ProductTypeRepositoryTests(CatalogFactory factory)
        : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IProductTypeRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProductType()
    {
        var productType = ProductType.Create("Electronics");

        // Act
        await _repository.CreateAsync(productType);
        await _repository.SaveChangesAsync();

        // Assert
        var specification = new GetProductTypeSpecification(productType.Id);
        var createdProductType = await _repository.SingleOrDefaultAsync(specification);

        // Act
        Assert.NotNull(createdProductType);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics");

        await _repository.CreateAsync(productType);
        await _repository.SaveChangesAsync();

        // Act
        _repository.Delete(productType);
        await _repository.SaveChangesAsync();

        // Assert
        var specification = new GetProductTypeSpecification(productType.Id);
        var deletedProductType = await _repository.SingleOrDefaultAsync(specification);
        Assert.Null(deletedProductType);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnProductTypes()
    {
        // Arrange
        var productType1 = ProductType.Create("Electronics");
        var productType2 = ProductType.Create("Clothing");

        await _repository.CreateAsync(productType1);
        await _repository.CreateAsync(productType2);
        await _repository.SaveChangesAsync();

        var specification = new GetProductTypesSpecification();

        // Act
        var productTypes = await _repository.ListAsync(specification);

        // Assert
        Assert.Contains(productTypes, pt => pt.Name == "Electronics");
        Assert.Contains(productTypes, pt => pt.Name == "Clothing");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics");

        await _repository.CreateAsync(productType);
        await _repository.SaveChangesAsync();
        var specification = new GetProductTypeSpecification(productType.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productType.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductTypeDoesNotExist()
    {
        // Arrange
        var specification = new GetProductTypeSpecification(new ProductTypeId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics");

        await _repository.CreateAsync(productType);
        await _repository.SaveChangesAsync();

        // Act
        productType.UpdateName("UpdatedElectronics");
        _repository.Update(productType);
        await _repository.SaveChangesAsync();

        // Assert
        var specification = new GetProductTypeSpecification(productType.Id);
        var updatedProductType = await _repository.SingleOrDefaultAsync(specification);

        Assert.NotNull(updatedProductType);
        Assert.Equal("UpdatedElectronics", updatedProductType.Name);
    }
}
