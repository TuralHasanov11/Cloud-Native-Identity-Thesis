using Microsoft.Extensions.Options;

namespace Catalog.Api.Extensions;

public partial class CatalogDbContextSeed(
    IWebHostEnvironment env,
    IOptions<CatalogOptions> settings,
    //ICatalogAI catalogAI,
    ILogger<CatalogDbContextSeed> logger) : IDbSeeder<CatalogDbContext>
{
    public async Task SeedAsync(CatalogDbContext context)
    {
        if (!context.Products.Any())
        {
            await AddBrands(context);
            await AddProductTypes(context);

            await context.SaveChangesAsync();

            await AddProducts(context);

            await context.SaveChangesAsync();
        }
    }

    private async Task AddProducts(CatalogDbContext context)
    {
        var products = new List<Product>();

        //if (catalogAI.IsEnabled)
        //{
        //    logger.LogInformation("Generating {NumItems} embeddings", products.Length);
        //    IReadOnlyList<Vector> embeddings = await catalogAI.GetEmbeddingsAsync(products);
        //    for (int i = 0; i < products.Length; i++)
        //    {
        //        products[i].Embedding = embeddings[i];
        //    }
        //}

        await context.Products.AddRangeAsync(products);
        logger.LogInformation("Seeded catalog with {NumItems} items", context.Products.Count());
    }

    private async Task AddProductTypes(CatalogDbContext context)
    {
        context.ProductTypes.RemoveRange(context.ProductTypes);
        await context.ProductTypes.AddRangeAsync([]);
        logger.LogInformation("Seeded catalog with {NumTypes} types", context.ProductTypes.Count());
    }

    private async Task AddBrands(CatalogDbContext context)
    {
        context.Brands.RemoveRange(context.Brands);
        await context.Brands.AddRangeAsync([]);
        logger.LogInformation("Seeded catalog with {NumBrands} brands", context.Brands.Count());
    }
}
