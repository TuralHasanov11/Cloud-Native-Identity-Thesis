namespace WebApp.Bff.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapBffApi(this IEndpointRouteBuilder app)
    {
        var identityApi = app.MapGroup("/api/identity");

        identityApi.MapGet("user-info", Identity.UserInfo.Handle)
            .AllowAnonymous()
            .WithName("UserInfo")
            .WithSummary("Get the current user's information.")
            .WithDescription("Get the current user's information.")
            .WithTags("Identity")
            .RequireRateLimiting("FixedRateLimitingPolicy");

        var basketApi = app.MapGroup("/api/basket");

        basketApi.MapGet("", Basket.GetByUser.Handle)
            .AllowAnonymous()
            .WithName("GetBasketByUser")
            .WithSummary("Get the current user's basket.")
            .WithDescription("Get the current user's basket.")
            .WithTags("Basket")
            .RequireRateLimiting("FixedRateLimitingPolicy");

        return app;
    }
}
