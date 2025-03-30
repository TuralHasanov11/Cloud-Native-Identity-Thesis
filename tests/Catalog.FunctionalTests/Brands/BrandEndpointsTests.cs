using System.Net;
using System.Net.Http.Json;

namespace Catalog.FunctionalTests.Brands;

public class BrandEndpointsTests : BaseEndpointTest
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    public BrandEndpointsTests(CatalogFactory factory)
        : base(factory)
    {
        Client = factory.HttpClient;
    }

    protected HttpClient Client { get; }

    [Fact]
    public async Task ListBrands_ShouldReturnBrands()
    {
        // Arrange

        // Act
        var response = await Client.GetAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands"));

        // Assert
        response.EnsureSuccessStatusCode();
        var brands = await response.Content.ReadFromJsonAsync<IEnumerable<BrandDto>>();
        Assert.NotNull(brands);
        Assert.NotEmpty(brands);
    }

    [Fact]
    public async Task ListBrands_ShouldReturnEmptyList_WhenNoBrandsExist()
    {
        // Act
        var response = await Client.GetAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands"));

        // Assert
        response.EnsureSuccessStatusCode();
        var brands = await response.Content.ReadFromJsonAsync<IEnumerable<BrandDto>>();
        Assert.NotNull(brands);
        Assert.Empty(brands);
    }

    [Fact(Skip = "Not Implemented Yet")]
    public async Task CreateBrand_ShouldCreateBrand()
    {
        // Arrange

        // Arrange
        var newBrand = new { Name = "New Brand" };

        // Act
        var response = await Client.PostAsJsonAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands"), newBrand);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdBrand = await response.Content.ReadFromJsonAsync<Brand>();
        Assert.NotNull(createdBrand);
        Assert.Equal(newBrand.Name, createdBrand.Name);
    }

    [Fact(Skip = "Not Implemented Yet")]
    public async Task CreateBrand_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var newBrand = new { Name = "" };

        // Act
        var response = await Client.PostAsJsonAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands"), newBrand);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(Skip = "Not Implemented Yet")]
    public async Task DeleteBrand_ShouldDeleteBrand()
    {
        // Arrange
        await using var scope = Factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        var brand = await dbContext.Brands.FirstAsync();

        // Act
        var response = await Client.DeleteAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands/{brand.Id}"));

        // Assert
        response.EnsureSuccessStatusCode();
        var deletedBrand = await dbContext.Brands.FindAsync(brand.Id);
        Assert.Null(deletedBrand);
    }

    [Fact(Skip = "Not Implemented Yet")]
    public async Task DeleteBrand_ShouldReturnNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        var nonExistentBrandId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands/{nonExistentBrandId}"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
