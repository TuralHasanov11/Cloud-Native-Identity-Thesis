namespace Catalog.IntegrationTests;

public static class CatalogDbContextExtensions
{
    public static Brand Brand1 => Brand.Create("Brand1");

    public static Brand Brand2 => Brand.Create("Brand2");

    public static async Task SeedDatabase(this CatalogDbContext dbContext)
    {
        if (!await dbContext.Brands.AnyAsync())
        {
            var brand1 = Brand1;
            var brand2 = Brand2;

            await dbContext.Brands.AddRangeAsync(brand1, brand2);
            await dbContext.SaveChangesAsync();
        }
    }
}
