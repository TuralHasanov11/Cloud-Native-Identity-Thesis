using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Features.Brands;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.FunctionalTests.Brands;

[Collection(nameof(EndpointTestCollection))]
public class BrandEndpointsTests : BaseEndpointTest
{
    protected const string ApiBaseUrl = "https://localhost:5103";

    protected BrandEndpointsTests(CatalogFactory factory)
        : base(factory)
    {
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    protected HttpClient Client { get; }

    [Fact]
    public async Task ListBrands_ShouldReturnBrands()
    {
        // Arrange
        await SeedDatabase();

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
        // Arrange
        await DbContext.Database.EnsureDeletedAsync();

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
        await SeedDatabase();
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
        await SeedDatabase();
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
        await SeedDatabase();
        var brand = await DbContext.Brands.FirstAsync();

        // Act
        var response = await Client.DeleteAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands/{brand.Id}"));

        // Assert
        response.EnsureSuccessStatusCode();
        var deletedBrand = await DbContext.Brands.FindAsync(brand.Id);
        Assert.Null(deletedBrand);
    }

    [Fact(Skip = "Not Implemented Yet")]
    public async Task DeleteBrand_ShouldReturnNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        await SeedDatabase();
        var nonExistentBrandId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync(new Uri($"{ApiBaseUrl}/api/catalog/brands/{nonExistentBrandId}"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
