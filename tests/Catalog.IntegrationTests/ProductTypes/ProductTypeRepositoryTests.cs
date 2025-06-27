namespace Catalog.IntegrationTests.ProductTypes;

public class ProductTypeRepositoryTests : BaseIntegrationTest
{
    private readonly IProductTypeRepository _repository;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

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
        await _repository.CreateAsync(productType, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var specification = new ProductTypeSpecification(productType.Id);
        var createdProductType = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Act
        Assert.NotNull(createdProductType);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics1");

        await _repository.CreateAsync(productType, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        _repository.Delete(productType);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var specification = new ProductTypeSpecification(productType.Id);
        var deletedProductType = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);
        Assert.Null(deletedProductType);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnProductTypes()
    {
        // Arrange
        var productType1 = ProductType.Create("Electronics2");
        var productType2 = ProductType.Create("Clothing2");

        await _repository.CreateAsync(productType1, _cancellationToken);
        await _repository.CreateAsync(productType2, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        var specification = new ProductTypeSpecification();

        // Act
        var productTypes = await _repository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.Contains(productTypes, pt => pt.Name == "Electronics2");
        Assert.Contains(productTypes, pt => pt.Name == "Clothing2");
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics3");

        await _repository.CreateAsync(productType, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);
        var specification = new ProductTypeSpecification(productType.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productType.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductTypeDoesNotExist()
    {
        // Arrange
        var specification = new ProductTypeSpecification(new ProductTypeId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProductType()
    {
        // Arrange
        var productType = ProductType.Create("Electronics4");

        await _repository.CreateAsync(productType, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        productType.UpdateName("UpdatedElectronics");
        _repository.Update(productType);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var specification = new ProductTypeSpecification(productType.Id);
        var updatedProductType = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        Assert.NotNull(updatedProductType);
        Assert.Equal("UpdatedElectronics", updatedProductType.Name);
    }
}
