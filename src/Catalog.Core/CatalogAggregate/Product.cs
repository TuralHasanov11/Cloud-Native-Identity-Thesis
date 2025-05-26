namespace Catalog.Core.CatalogAggregate;

public sealed class Product : EntityBase<ProductId>
{
    private Product(
        string name,
        string description,
        decimal price,
        ProductTypeId productTypeId,
        BrandId brandId,
        int availableStock,
        int restockThreshold,
        int maxStockThreshold)
        : base(new ProductId(Guid.CreateVersion7()))
    {
        Name = name;
        Description = description;
        Price = price;
        ProductTypeId = productTypeId;
        BrandId = brandId;
        AvailableStock = availableStock;
        RestockThreshold = restockThreshold;
        MaxStockThreshold = maxStockThreshold;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public Uri? PictureFileName { get; private set; }

    public ProductTypeId ProductTypeId { get; private set; }

    public ProductType ProductType { get; }

    public BrandId BrandId { get; private set; }

    public Brand Brand { get; }

    public int AvailableStock { get; private set; }

    public int RestockThreshold { get; private set; }

    public int MaxStockThreshold { get; private set; }

    ///// <summary>Optional embedding for the catalog item's description.</summary>
    //[JsonIgnore]
    //public Vector Embedding { get; set; }

    public bool OnReorder { get; private set; }

    public int RemoveStock(int quantityDesired)
    {
        if (AvailableStock == 0)
        {
            throw new CatalogException($"Empty stock, product item {Name} is sold out");
        }

        const int minQuantityThreshold = 0;

        if (quantityDesired <= minQuantityThreshold)
        {
            throw new CatalogException($"Item units desired should be greater than {minQuantityThreshold}");
        }

        int removed = Math.Min(quantityDesired, AvailableStock);

        AvailableStock -= removed;

        return removed;
    }

    public int AddStock(int quantity)
    {
        int original = AvailableStock;

        if ((AvailableStock + quantity) > MaxStockThreshold)
        {
            AvailableStock += MaxStockThreshold - AvailableStock;
        }
        else
        {
            AvailableStock += quantity;
        }

        OnReorder = false;

        return AvailableStock - original;
    }

    public void SetPictureUri(Uri pictureUri)
    {
        ArgumentNullException.ThrowIfNull(pictureUri, nameof(pictureUri));

        PictureFileName = pictureUri;
    }

    public void Update(
        string name,
        string description,
        decimal price,
        ProductTypeId productTypeId,
        BrandId brandId,
        int availableStock,
        int restockThreshold,
        int maxStockThreshold)
    {
        Name = name;
        Description = description;
        Price = price;
        ProductTypeId = productTypeId;
        BrandId = brandId;
        AvailableStock = availableStock;
        RestockThreshold = restockThreshold;
        MaxStockThreshold = maxStockThreshold;
    }

    public static Product Create(
        string name,
        string description,
        decimal price,
        ProductTypeId productTypeId,
        BrandId brandId,
        int availableStock,
        int restockThreshold,
        int maxStockThreshold)
    {
        return new Product(
            name,
            description,
            price,
            productTypeId,
            brandId,
            availableStock,
            restockThreshold,
            maxStockThreshold);
    }
}
