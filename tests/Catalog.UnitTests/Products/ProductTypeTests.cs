using Catalog.Api.Features.ProductTypes;

namespace Catalog.UnitTests.Products;

public class ProductTypeTests
{
    [Fact]
    public void Create_ShouldInitializeProductType()
    {
        // Arrange
        const string name = "ProductType1";

        // Act
        var productType = ProductType.Create(name);

        // Assert
        Assert.Equal(name, productType.Name);
        Assert.NotEqual(Guid.Empty, productType.Id.Value);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        const string name = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => ProductType.Create(name));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var name = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ProductType.Create(name));
        Assert.Equal("The name cannot be empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateName_ShouldModifyProductTypeName()
    {
        // Arrange
        var productType = ProductType.Create("ProductType1");
        const string newName = "UpdatedProductType";

        // Act
        productType.UpdateName(newName);

        // Assert
        Assert.Equal(newName, productType.Name);
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        var productType = ProductType.Create("ProductType1");
        const string newName = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => productType.UpdateName(newName));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var productType = ProductType.Create("ProductType1");
        var newName = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => productType.UpdateName(newName));
        Assert.Equal("The name cannot be empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void ToProductTypeDto_ShouldReturnProductTypeDto()
    {
        // Arrange
        var productType = ProductType.Create("ProductType1");

        // Act
        var productTypeDto = productType.ToProductTypeDto();

        // Assert
        Assert.Equal(productType.Id, productTypeDto.Id);
        Assert.Equal(productType.Name, productTypeDto.Name);
    }
}
