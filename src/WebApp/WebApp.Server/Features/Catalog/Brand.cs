namespace WebApp.Server.Features.Catalog;

public record Brand(Guid Id, string Name)
{
    public static IEnumerable<Brand> Empty()
    {
        return [];
    }
}
