using FluentValidation;
using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Server.Features.Catalog;

namespace WebApp.Server.Features.Basket;

public static class UpdateByUser
{
    public static async Task<Results<Ok<IEnumerable<BasketItem>>, Conflict<string>, ProblemHttpResult>> Handle(
        ICatalogService catalogService,
        BasketService basketService,
        UpdateBasketByUserRequest request)
    {
        try
        {
            var productIds = request.Items
                .Select(x => x.ProductId)
                .Distinct();

            var products = await catalogService.GetProducts(productIds);

            if (products.Count() != productIds.Count())
            {
                return TypedResults.Conflict("Some products are not available in the catalog.");
            }

            var items = await basketService.UpdateBasketAsync(request.Items);

            var result = items.Select(b => new
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

            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }
}
