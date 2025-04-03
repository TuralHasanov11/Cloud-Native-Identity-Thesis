namespace Catalog.IntegrationTests.Products;

[Collection(nameof(IntegrationTestCollection))]
public class ProductRepositoryTests : BaseIntegrationTest
{
    private readonly IProductRepository _repository;

    public ProductRepositoryTests(CatalogFactory factory)
        : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IProductRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        // Arrange

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        // Act
        await _repository.CreateAsync(product);
        await _repository.SaveChangesAsync();

        // Assert
        var createdProduct = await _repository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(product.Id));

        Assert.NotNull(createdProduct);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProduct()
    {
        // Arrange

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await _repository.CreateAsync(product);
        await _repository.SaveChangesAsync();

        // Act
        _repository.Delete(product);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedProduct = await _repository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(product.Id));

        Assert.Null(deletedProduct);
    }

    //[Fact]
    //public async Task ListAsync_ShouldReturnProducts()
    //{
    //    // Arrange
    //    var DbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
    //

    //    var repository = new ProductRepository(DbContext);

    //    var product1 = Product.Create(
    //        "Product1",
    //        "Description1",
    //        10.0m,
    //        new ProductTypeId(Guid.NewGuid()),
    //        new BrandId(Guid.NewGuid()),
    //        100,
    //        10,
    //        200);

    //    var product2 = Product.Create(
    //        "Product2",
    //        "Description2",
    //        20.0m,
    //        new ProductTypeId(Guid.NewGuid()),
    //        new BrandId(Guid.NewGuid()),
    //        200,
    //        20,
    //        400);

    //    await repository.CreateAsync(product1);
    //    await repository.CreateAsync(product2);
    //    await repository.SaveChangesAsync();

    //    var specification = new GetProductsSpecification();

    //    // Act
    //    var products = await repository.ListAsync(specification);

    //    // Assert
    //    Assert.Contains(products, p => p.Id == product1.Id);
    //    Assert.Contains(products, p => p.Id == product2.Id);
    //}

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProduct()
    {
        // Arrange
        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await _repository.CreateAsync(product);
        await _repository.SaveChangesAsync();
        var specification = new GetProductByIdSpecification(product.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var specification = new GetProductByIdSpecification(new ProductId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProduct()
    {
        // Arrange
        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await _repository.CreateAsync(product);
        await _repository.SaveChangesAsync();

        // Act
        product.Update(
            "UpdatedProduct",
            "UpdatedDescription",
            15.0m,
            product.ProductTypeId,
            product.BrandId,
            150,
            15,
            300);
        _repository.Update(product);
        await _repository.SaveChangesAsync();

        // Assert
        var updatedProduct = await _repository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(product.Id));

        Assert.NotNull(updatedProduct);
        Assert.Equal("UpdatedProduct", updatedProduct.Name);
        Assert.Equal("UpdatedDescription", updatedProduct.Description);
        Assert.Equal(15.0m, updatedProduct.Price);
    }
}
