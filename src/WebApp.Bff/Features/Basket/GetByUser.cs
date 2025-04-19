using ServiceDefaults.Identity;
using WebApp.Bff.Features.Catalog;

namespace WebApp.Bff.Features.Basket;

public static class GetByUser
{
    public static async Task<IResult> Handle(
        IIdentityService identityService,
        ICatalogService catalogService,
        BasketService basketService,
        CancellationToken _)
    {
        var userId = identityService.GetUser()?.GetUserId();
        if (userId is null)
        {
            return Results.Unauthorized();
        }

        var basket = await basketService.GetBasketAsync();

        if (basket is null)
        {
            return Results.NotFound();
        }

        var productIds = basket
            .Select(x => x.ProductId)
            .Distinct();

        var products = await catalogService.GetProducts(productIds);


        return Results.Ok(products);
    }
}
