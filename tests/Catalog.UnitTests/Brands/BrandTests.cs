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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_ShouldThrowException_WhenNameIsNull(string name)
    {
        // Act & Assert
        Assert.Throws<BrandNameEmptyException>(() => Brand.Create(name));
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateName_ShouldThrowException_WhenNameIsNull(string newName)
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        // Act & Assert
        Assert.Throws<BrandNameEmptyException>(() => brand.UpdateName(newName));
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

    [Theory]
    [InlineData("A very long brand name that exceeds the maximum length of one hundred characters which should throw an exception")]
    public void Create_ShouldThrowException_WhenNameIsTooLong(string name)
    {
        // Act & Assert
        Assert.Throws<BrandNameTooLongException>(() => Brand.Create(name));
    }

    [Theory]
    [InlineData("A very long brand name that exceeds the maximum length of one hundred characters which should throw an exception")]
    public void UpdateName_ShouldThrowException_WhenNameIsTooLong(string newName)
    {
        // Arrange
        var brand = Brand.Create("Brand1");

        // Act & Assert
        Assert.Throws<BrandNameTooLongException>(() => brand.UpdateName(newName));
    }
}
