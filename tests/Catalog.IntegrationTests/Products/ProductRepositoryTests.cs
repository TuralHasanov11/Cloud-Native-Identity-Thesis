namespace Catalog.IntegrationTests.Products;

public class ProductRepositoryTests : IClassFixture<CatalogFactory>
{
    private readonly CatalogFactory _factory;

    public ProductRepositoryTests(CatalogFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductRepository(dbContext);
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
        await repository.CreateAsync(product);
        await repository.SaveChangesAsync();

        // Assert
        var createdProduct = await dbContext.Products.FindAsync(product.Id);
        Assert.NotNull(createdProduct);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProduct()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductRepository(dbContext);
        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await repository.CreateAsync(product);
        await repository.SaveChangesAsync();

        // Act
        repository.Delete(product);
        await repository.SaveChangesAsync();

        // Assert
        var deletedProduct = await dbContext.Products.FindAsync(product.Id);
        Assert.Null(deletedProduct);
    }

    //[Fact]
    //public async Task ListAsync_ShouldReturnProducts()
    //{
    //    // Arrange
    //    var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
    //    await dbContext.SeedDatabase();

    //    var repository = new ProductRepository(dbContext);

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
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductRepository(dbContext);
        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await repository.CreateAsync(product);
        await repository.SaveChangesAsync();
        var specification = new GetProductByIdSpecification(product.Id);

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductRepository(dbContext);
        var specification = new GetProductByIdSpecification(new ProductId(Guid.NewGuid()));

        // Act
        var result = await repository.SingleOrDefaultAsync(specification);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProduct()
    {
        // Arrange
        var dbContext = _factory.Services.GetRequiredService<CatalogDbContext>();
        await dbContext.SeedDatabase();

        var repository = new ProductRepository(dbContext);
        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            new ProductTypeId(Guid.NewGuid()),
            new BrandId(Guid.NewGuid()),
            100,
            10,
            200);

        await repository.CreateAsync(product);
        await repository.SaveChangesAsync();

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
        repository.Update(product);
        await repository.SaveChangesAsync();

        // Assert
        var updatedProduct = await dbContext.Products.FindAsync(product.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal("UpdatedProduct", updatedProduct.Name);
        Assert.Equal("UpdatedDescription", updatedProduct.Description);
        Assert.Equal(15.0m, updatedProduct.Price);
    }
}
