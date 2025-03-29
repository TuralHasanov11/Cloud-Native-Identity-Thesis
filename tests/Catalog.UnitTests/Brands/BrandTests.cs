using Catalog.Api.Features.Brands;

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
        Assert.Throws<ArgumentNullException>(() => Brand.Create(name));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var name = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Brand.Create(name));
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
        Assert.Throws<ArgumentNullException>(() => brand.UpdateName(newName));
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var brand = Brand.Create("Brand1");
        var newName = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => brand.UpdateName(newName));
    }

    [Fact]
    public void ToBrandDto_ShouldMapBrandToBrandDto()
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        // Act
        var brandDto = brand.ToBrandDto();

        // Assert
        Assert.Equal(brand.Id, brandDto.Id);
        Assert.Equal(brand.Name, brandDto.Name);
    }

    [Fact]
    public void ToBrandDto_ShouldThrowException_WhenBrandIsNull()
    {
        // Arrange
        Brand? brand = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => brand?.ToBrandDto());
    }
}
