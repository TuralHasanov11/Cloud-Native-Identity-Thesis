using Catalog.Api.Features.Products;

namespace Catalog.UnitTests.Products;

public class ProductTests
{
    [Fact]
    public void Create_ShouldInitializeProduct()
    {
        // Arrange
        const string name = "Product1";
        const string description = "Description1";
        const decimal price = 10.0m;
        var productTypeId = new ProductTypeId(Guid.NewGuid());
        var brandId = new BrandId(Guid.NewGuid());
        const int availableStock = 100;
        const int restockThreshold = 10;
        const int maxStockThreshold = 200;

        // Act
        var product = Product.Create(name, description, price, productTypeId, brandId, availableStock, restockThreshold, maxStockThreshold);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(productTypeId, product.ProductTypeId);
        Assert.Equal(brandId, product.BrandId);
        Assert.Equal(availableStock, product.AvailableStock);
        Assert.Equal(restockThreshold, product.RestockThreshold);
        Assert.Equal(maxStockThreshold, product.MaxStockThreshold);
    }

    [Fact]
    public void RemoveStock_ShouldDecreaseAvailableStock()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);
        const int quantityDesired = 20;

        // Act
        var removed = product.RemoveStock(quantityDesired);

        // Assert
        Assert.Equal(quantityDesired, removed);
        Assert.Equal(80, product.AvailableStock);
    }

    [Fact]
    public void RemoveStock_ShouldThrowException_WhenStockIsEmpty()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 0, 10, 200);

        // Act & Assert
        var exception = Assert.Throws<CatalogException>(() => product.RemoveStock(10));
        Assert.Equal("Empty stock, product item Product1 is sold out", exception.Message);
    }

    [Fact]
    public void RemoveStock_ShouldThrowException_WhenQuantityDesiredIsInvalid()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);

        // Act & Assert
        var exception = Assert.Throws<CatalogException>(() => product.RemoveStock(0));
        Assert.Equal("Item units desired should be greater than 0", exception.Message);
    }

    [Fact]
    public void AddStock_ShouldIncreaseAvailableStock()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);
        const int quantity = 50;

        // Act
        var added = product.AddStock(quantity);

        // Assert
        Assert.Equal(quantity, added);
        Assert.Equal(150, product.AvailableStock);
    }

    [Fact]
    public void AddStock_ShouldNotExceedMaxStockThreshold()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 150, 10, 200);
        const int quantity = 100;

        // Act
        var added = product.AddStock(quantity);

        // Assert
        Assert.Equal(50, added);
        Assert.Equal(200, product.AvailableStock);
    }

    [Fact]
    public void SetPictureUri_ShouldSetPictureUri()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);
        var pictureUri = new Uri("http://example.com/picture.jpg");

        // Act
        product.SetPictureUri(pictureUri);

        // Assert
        Assert.Equal(pictureUri, product.PictureFileName);
    }

    [Fact]
    public void Update_ShouldModifyProductProperties()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);
        const string newName = "UpdatedProduct";
        const string newDescription = "UpdatedDescription";
        const decimal newPrice = 15.0m;
        var newProductTypeId = new ProductTypeId(Guid.NewGuid());
        var newBrandId = new BrandId(Guid.NewGuid());
        const int newAvailableStock = 150;
        const int newRestockThreshold = 15;
        const int newMaxStockThreshold = 300;

        // Act
        product.Update(newName, newDescription, newPrice, newProductTypeId, newBrandId, newAvailableStock, newRestockThreshold, newMaxStockThreshold);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newPrice, product.Price);
        Assert.Equal(newProductTypeId, product.ProductTypeId);
        Assert.Equal(newBrandId, product.BrandId);
        Assert.Equal(newAvailableStock, product.AvailableStock);
        Assert.Equal(newRestockThreshold, product.RestockThreshold);
        Assert.Equal(newMaxStockThreshold, product.MaxStockThreshold);
    }

    [Fact]
    public void ToProductDto_ShouldMapProductToProductDto()
    {
        // Arrange
        var product = Product.Create("Product1", "Description1", 10.0m, new ProductTypeId(Guid.NewGuid()), new BrandId(Guid.NewGuid()), 100, 10, 200);

        // Act
        var productDto = product.ToProductDto();

        // Assert
        Assert.Equal(product.Id, productDto.Id);
        Assert.Equal(product.Name, productDto.Name);
        Assert.Equal(product.Description, productDto.Description);
        Assert.Equal(product.Price, productDto.Price);
        Assert.Equal(product.ProductTypeId, productDto.ProductTypeId);
        Assert.Equal(product.BrandId, productDto.BrandId);
        Assert.Equal(product.AvailableStock, productDto.AvailableStock);
        Assert.Equal(product.RestockThreshold, productDto.RestockThreshold);
        Assert.Equal(product.MaxStockThreshold, productDto.MaxStockThreshold);
    }


}
