namespace Catalog.UnitTests.Brands;

public class BrandTests
{
    [Fact]
    public void Create_ShouldInitializeBrand()
    {
        // Arrange
        var name = "Brand1";

        // Act
        var brand = Brand.Create(name);

        // Assert
        Assert.Equal(name, brand.Name);
        Assert.NotEqual(Guid.Empty, brand.Id.Value);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        string name = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Brand.Create(name));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var name = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Brand.Create(name));
        Assert.Equal("The name cannot be empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateName_ShouldModifyBrandName()
    {
        // Arrange
        var brand = Brand.Create("Brand1");
        var newName = "UpdatedBrand";

        // Act
        brand.UpdateName(newName);

        // Assert
        Assert.Equal(newName, brand.Name);
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        var brand = Brand.Create("Brand1");
        string newName = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => brand.UpdateName(newName));
        Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var brand = Brand.Create("Brand1");
        var newName = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => brand.UpdateName(newName));
        Assert.Equal("The name cannot be empty. (Parameter 'name')", exception.Message);
    }
}
