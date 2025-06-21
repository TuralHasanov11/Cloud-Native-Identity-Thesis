using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Server.Features.Catalog;

namespace WebApp.Server.Features.Basket;

public static class GetByUser
{
    public static async Task<Results<Ok<IEnumerable<BasketItem>>, ProblemHttpResult>> Handle(
        ICatalogService catalogService,
        BasketService basketService,
        CancellationToken _)
    {
        try
        {
            var basket = await basketService.GetBasketAsync();

            if (basket is null)
            {
                return TypedResults.Ok(BasketItem.Empty());
            }

            var productIds = basket
                .Select(x => x.ProductId)
                .Distinct();

            var products = await catalogService.GetProducts(productIds);

            var items = basket.Select(b => new
            {
                b.ProductId,
                b.Quantity,
                Product = products.First(p => p.Id.ToString() == b.ProductId)
            })
                .Where(x => x.Product is not null)
                .Select(x => new BasketItem(
                    Guid.Parse(x.ProductId),
                    x.Product.Name,
                    x.Product.Price,
                    x.Quantity));

            return TypedResults.Ok(items);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }
}
