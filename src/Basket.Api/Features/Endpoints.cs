using Basket.Api.Features.Basket;
using ServiceDefaults.Identity;

namespace Basket.Api.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapBasketEndpoints(this IEndpointRouteBuilder api)
    {
        api.MapGet("/api/basket", async (IBasketRepository repository, IIdentityService identityService) =>
        {
            var user = identityService.GetUser();
            var userId = user?.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var basket = await repository.GetBasketAsync(userId);

            if (basket is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(MapToCustomerBasketResponse(basket));
        })
        .AllowAnonymous()
        .WithName("GetBasket")
        .Produces<CustomerBasketResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        api.MapPost("/api/basket", async (UpdateBasketRequest request, IBasketRepository repository, IIdentityService identityService) =>
        {
            var user = identityService.GetUser();
            var userId = user?.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var customerBasket = MapToCustomerBasket(userId, request);
            var updatedBasket = await repository.UpdateBasketAsync(customerBasket);

            if (updatedBasket is null)
            {
                return Results.NotFound($"Basket with buyer id {userId} does not exist.");
            }

            return Results.Ok(MapToCustomerBasketResponse(updatedBasket));
        })
        .AllowAnonymous()
        .WithName("UpdateBasket")
        .Produces<CustomerBasketResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        api.MapDelete("/api/basket", async (IBasketRepository repository, IIdentityService identityService) =>
        {
            var user = identityService.GetUser();
            var userId = user?.GetUserId();

            await repository.DeleteBasketAsync(userId);
            return Results.NoContent();
        })
        .AllowAnonymous()
        .WithName("DeleteBasket")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized);

        return api;
    }

    private static CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
    {
        var response = new CustomerBasketResponse();

        foreach (var item in customerBasket.Items)
        {
            response.Items.Add(new BasketItem()
            {
                ProductId = item.ProductId.ToString(),
                Quantity = item.Quantity,
            });
        }

        return response;
    }

    private static CustomerBasket MapToCustomerBasket(string userId, UpdateBasketRequest request)
    {
        var response = new CustomerBasket
        {
            CustomerId = userId
        };

        foreach (var item in request.Items)
        {
            response.Items.Add(new()
            {
                ProductId = Guid.Parse(item.ProductId),
                Quantity = item.Quantity,
            });
        }

        return response;
    }
}

public class BasketItem
{
    public string ProductId { get; set; }

    public int Quantity { get; set; }
}

public class CustomerBasketResponse
{
    public List<BasketItem> Items { get; init; } = new();
}

public class UpdateBasketRequest
{
    public List<BasketItem> Items { get; init; } = new();
}
