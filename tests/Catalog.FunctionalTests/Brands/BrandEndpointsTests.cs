using System.Net;
using System.Net.Http.Json;

namespace Catalog.FunctionalTests.Brands;

public class BrandEndpointsTests : BaseEndpointTest
{
    public BrandEndpointsTests(CatalogFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task ListBrands_ShouldReturnBrands()
    {
        // Arrange  
        await DbContext.Brands.AddRangeAsync(Brand.Create("Brand1"), Brand.Create("Brand2"));
        await DbContext.SaveChangesAsync();

        // Act  
        var response = await HttpClient.GetAsync(new Uri("api/catalog/brands", UriKind.Relative));

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
        var response = await HttpClient.GetAsync(new Uri("api/catalog/brands", UriKind.Relative));

        // Assert  
        response.EnsureSuccessStatusCode();
        var brands = await response.Content.ReadFromJsonAsync<IEnumerable<BrandDto>>();
        Assert.NotNull(brands);
        Assert.Empty(brands);
    }

    [Fact]
    public async Task CreateBrand_ShouldCreateBrand()
    {
        // Arrange  
        var newBrand = new { Name = "New Brand" };

        // Act  
        var response = await HttpClient.PostAsJsonAsync("api/catalog/brands", newBrand);

        // Assert  
        response.EnsureSuccessStatusCode();
        var createdBrand = await response.Content.ReadFromJsonAsync<Brand>();
        Assert.NotNull(createdBrand);
        Assert.Equal(newBrand.Name, createdBrand.Name);
    }

    [Fact]
    public async Task CreateBrand_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange  
        var newBrand = new { Name = "" };

        // Act  
        var response = await HttpClient.PostAsJsonAsync("api/catalog/brands", newBrand);

        // Assert  
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBrand_ShouldDeleteBrand()
    {
        // Arrange  
        await DbContext.Brands.AddAsync(Brand.Create("Brand1"));
        await DbContext.SaveChangesAsync();
        var brand = await DbContext.Brands.FirstAsync();

        // Act  
        var response = await HttpClient.DeleteAsync($"api/catalog/brands/{brand.Id}");

        // Assert  
        response.EnsureSuccessStatusCode();
        var deletedBrand = await DbContext.Brands.FindAsync(brand.Id);
        Assert.Null(deletedBrand);
    }

    [Fact]
    public async Task DeleteBrand_ShouldReturnNotFound_WhenBrandDoesNotExist()
    {
        // Arrange  
        var nonExistentBrandId = Guid.NewGuid();

        // Act  
        var response = await HttpClient.DeleteAsync($"api/catalog/brands/{nonExistentBrandId}");

        // Assert  
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
