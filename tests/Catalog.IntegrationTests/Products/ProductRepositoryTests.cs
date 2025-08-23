namespace Catalog.IntegrationTests.Products;

public class ProductRepositoryTests : BaseIntegrationTest
{
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public ProductRepositoryTests(CatalogFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await SeedDatabase(DbContext);

        var brand = await DbContext.Brands.FirstAsync(_cancellationToken);
        var productType = await DbContext.ProductTypes.FirstAsync(_cancellationToken);

        var product = Product.Create(
            "Product1",
            "Description1",
            10.0m,
            productType.Id,
            brand.Id,
            100,
            10,
            200);

        // Act
        await _repository.CreateAsync(product, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var createdProduct = await _repository.SingleOrDefaultAsync(
            new ProductSpecification(product.Id), _cancellationToken);

        Assert.NotNull(createdProduct);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProduct()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await SeedDatabase(DbContext);

        var brand = await DbContext.Brands.FirstAsync(_cancellationToken);
        var productType = await DbContext.ProductTypes.FirstAsync(_cancellationToken);

        var product = Product.Create(
            "Product2",
            "Description1",
            10.0m,
            productType.Id,
            brand.Id,
            100,
            10,
            200);

        await _repository.CreateAsync(product, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

        // Act
        _repository.Delete(product);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedProduct = await _repository.SingleOrDefaultAsync(
            new ProductSpecification(product.Id),
            _cancellationToken);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnProducts()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await SeedDatabase(DbContext);

        var existingProducts = await DbContext.Products.ToListAsync(_cancellationToken);

        // Act
        var specification = new ProductSpecification();
        var products = await _repository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.True(products.Any());
        Assert.Equal(existingProducts.Count, products.Count());
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProduct()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await SeedDatabase(DbContext);

        // Arrange
        var brand = await DbContext.Brands.FirstAsync(_cancellationToken);
        var productType = await DbContext.ProductTypes.FirstAsync(_cancellationToken);

        var product = Product.Create(
            "Product2",
            "Description1",
            10.0m,
            productType.Id,
            brand.Id,
            100,
            10,
            200);

        await _repository.CreateAsync(product, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);
        var specification = new ProductSpecification(product.Id);

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var specification = new ProductSpecification(new ProductId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProduct()
    {
        // Arrange
        var _repository = Scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await SeedDatabase(DbContext);

        var brand = await DbContext.Brands.FirstAsync(_cancellationToken);
        var productType = await DbContext.ProductTypes.FirstAsync(_cancellationToken);

        var product = Product.Create(
            "Product3",
            "Description1",
            10.0m,
            productType.Id,
            brand.Id,
            100,
            10,
            200);

        await _repository.CreateAsync(product, _cancellationToken);
        await _repository.SaveChangesAsync(_cancellationToken);

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
        await _repository.SaveChangesAsync(_cancellationToken);

        // Assert
        var updatedProduct = await _repository.SingleOrDefaultAsync(
            new ProductSpecification(product.Id),
            _cancellationToken);

        Assert.NotNull(updatedProduct);
        Assert.Equal("UpdatedProduct", updatedProduct.Name);
        Assert.Equal("UpdatedDescription", updatedProduct.Description);
        Assert.Equal(15.0m, updatedProduct.Price);
    }

    public IEnumerable<Brand> GetBrands()
    {
        yield return Brand.Create("Brand1");
        yield return Brand.Create("Brand2");
    }

    public IEnumerable<ProductType> GetProductTypes()
    {
        yield return ProductType.Create("ProductType1");
        yield return ProductType.Create("ProductType2");
    }

    public async Task SeedDatabase(CatalogDbContext dbContext)
    {
        if (!await dbContext.Brands.AnyAsync())
        {
            var brands = GetBrands();
            await dbContext.AddRangeAsync(brands);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.ProductTypes.AnyAsync())
        {
            var productTypes = GetProductTypes();
            await dbContext.AddRangeAsync(productTypes);
            await dbContext.SaveChangesAsync();
        }
    }
}
