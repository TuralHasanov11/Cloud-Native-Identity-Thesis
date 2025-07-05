using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Extensions;

public class CatalogDbContextSeed(
    IWebHostEnvironment env,
    ILogger<CatalogDbContextSeed> logger) : IDbSeeder<CatalogDbContext>
{
    public async Task SeedAsync(CatalogDbContext context)
    {
        if (!await context.Products.AnyAsync())
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
        var contentRootPath = env.ContentRootPath;
        var sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
        var sourceJson = await File.ReadAllTextAsync(sourcePath);

        var products = JsonSerializer.Deserialize<List<ProductDto>>(sourceJson);

        if (products != null)
        {
            var brands = await context.Brands.ToListAsync();
            var productTypes = await context.ProductTypes.ToListAsync();
            var existingProductNames = await context.Products.Select(p => p.Name).ToListAsync();

            var productEntities = products
                .Where(p => !existingProductNames.Contains(p.Name))
                .Select(p =>
                {
                    var brand = brands.FirstOrDefault(b => b.Name == p.Brand);
                    var productType = productTypes.FirstOrDefault(pt => pt.Name == p.Type);

                    if (brand == null || productType == null)
                    {
                        throw new InvalidOperationException("Brand or ProductType not found for product.");
                    }

                    return Product.Create(
                        p.Name,
                        p.Description,
                        p.Price,
                        productType.Id,
                        brand.Id,
                        100, // AvailableStock
                        10, // RestockThreshold
                        1000 // MaxStockThreshold
                    );
                }).ToList();

            await context.Products.AddRangeAsync(productEntities);
            logger.LogInformation("Seeded catalog with {NumItems} items", await context.Products.CountAsync());
        }
    }

    private async Task AddProductTypes(CatalogDbContext context)
    {
        var contentRootPath = env.ContentRootPath;

        if (!await context.ProductTypes.AnyAsync())
        {
            var sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
            var sourceJson = await File.ReadAllTextAsync(sourcePath);

            var products = JsonSerializer.Deserialize<List<ProductDto>>(sourceJson);

            if (products != null)
            {
                var productTypeNames = products.Select(p => p.Type).Distinct().ToList();
                var productTypes = productTypeNames.ConvertAll(ProductType.Create);

                await context.ProductTypes.AddRangeAsync(productTypes);
                logger.LogInformation("Seeded catalog with {NumTypes} types", await context.ProductTypes.CountAsync());
            }
        }
    }

    private async Task AddBrands(CatalogDbContext context)
    {
        var contentRootPath = env.ContentRootPath;

        if (!await context.Brands.AnyAsync())
        {
            var sourcePath = Path.Combine(contentRootPath, "Setup", "catalog.json");
            var sourceJson = await File.ReadAllTextAsync(sourcePath);

            var products = JsonSerializer.Deserialize<List<ProductDto>>(sourceJson);

            if (products != null)
            {
                var brandNames = products.Select(p => p.Brand).Distinct().ToList();
                var brands = brandNames.ConvertAll(Brand.Create);

                await context.Brands.AddRangeAsync(brands);
                logger.LogInformation("Seeded catalog with {NumBrands} brands", await context.Brands.CountAsync());
            }
        }
    }
}

public class ProductDto
{
    public int Id { get; set; }

    public string Type { get; set; }

    public string Brand { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }
}
