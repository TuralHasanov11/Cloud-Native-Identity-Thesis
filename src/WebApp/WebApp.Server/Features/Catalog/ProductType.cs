namespace WebApp.Server.Features.Catalog;

public record ProductType(Guid Id, string Name)
{
    public static IEnumerable<ProductType> Empty()
    {
        return [];
    }
}
