using WebApp.Bff.Features.Catalog;

namespace WebApp.Bff.Features.Basket;

public static class GetByUser
{
    public static async Task<IResult> Handle(
        ICatalogService catalogService,
        BasketService basketService,
        CancellationToken _)
    {
        try
        {
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
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
