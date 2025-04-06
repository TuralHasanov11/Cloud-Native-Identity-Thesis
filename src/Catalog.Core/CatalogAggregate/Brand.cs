namespace Catalog.Core.CatalogAggregate;

public sealed class Brand : EntityBase<BrandId>
{
    private Brand(string name)
        : base(new BrandId(Guid.CreateVersion7()))
    {
        ValidateName(name);

        Name = name;
    }

    public string Name { get; private set; }

    public void UpdateName(string name)
    {
        ValidateName(name);

        Name = name;
    }

    public static Brand Create(string name)
    {
        return new Brand(name);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BrandNameEmptyException();
        }

        const int maxLength = 100;

        if (name.Length > maxLength)
        {
            throw new BrandNameTooLongException(name.Length, maxLength);
        }
    }
}
