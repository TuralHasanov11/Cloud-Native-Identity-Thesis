namespace Catalog.IntegrationTests.Products;

public class ProductRepositoryTests : BaseIntegrationTest
{
    private readonly IProductRepository _repository;
    private readonly CancellationToken _cancellationToken = TestContext.Current.CancellationToken;

    public ProductRepositoryTests(CatalogFactory factory)
        : base(factory)
    {
        _repository = factory.Services.GetRequiredService<IProductRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        // Arrange
        await SeedDatabase();

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
            new GetProductSpecification(product.Id), _cancellationToken);

        Assert.NotNull(createdProduct);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProduct()
    {
        // Arrange
        await SeedDatabase();

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
            new GetProductSpecification(product.Id),
            _cancellationToken);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnProducts()
    {
        // Arrange
        await SeedDatabase();

        var existingProducts = await DbContext.Products.ToListAsync(_cancellationToken);

        // Act
        var specification = new GetProductsSpecification();
        var products = await _repository.ListAsync(specification, _cancellationToken);

        // Assert
        Assert.True(products.Any());
        Assert.Equal(existingProducts.Count, products.Count());
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldReturnProduct()
    {
        await SeedDatabase();

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
        var specification = new GetProductSpecification(product.Id);

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
        var specification = new GetProductSpecification(new ProductId(Guid.NewGuid()));

        // Act
        var result = await _repository.SingleOrDefaultAsync(specification, _cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldModifyProduct()
    {
        // Arrange
        await SeedDatabase();

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
            new GetProductSpecification(product.Id),
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

    public async Task SeedDatabase()
    {
        if (!await DbContext.Brands.AnyAsync())
        {
            var brands = GetBrands();
            await DbContext.AddRangeAsync(brands);
            await DbContext.SaveChangesAsync();
        }

        if (!await DbContext.ProductTypes.AnyAsync())
        {
            var productTypes = GetProductTypes();
            await DbContext.AddRangeAsync(productTypes);
            await DbContext.SaveChangesAsync();
        }
    }
}
