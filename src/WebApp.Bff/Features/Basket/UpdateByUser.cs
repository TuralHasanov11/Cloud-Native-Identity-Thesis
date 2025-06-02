using FluentValidation;
using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Bff.Features.Catalog;

namespace WebApp.Bff.Features.Basket;

public static class UpdateByUser
{
    public static async Task<Results<Ok<IEnumerable<BasketQuantity>>, Conflict<string>, ProblemHttpResult>> Handle(
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

            return TypedResults.Ok(items);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }
}
