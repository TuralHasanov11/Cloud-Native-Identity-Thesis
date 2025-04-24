using Microsoft.Identity.Web;
using ServiceDefaults.Identity;

namespace WebApp.Bff.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapBffApi(this IEndpointRouteBuilder app)
    {
        var identityRoutes = app.MapGroup("/identity");
        identityRoutes.MapGet("login", Identity.Login.Handle);

        var identityApi = app.MapGroup("/api/identity");

        identityApi.MapGet("user-info", Identity.UserInfo.Handle)
            .WithName("UserInfo")
            .WithSummary("Get the current user's information.")
            .WithDescription("Get the current user's information.")
            .WithTags("Identity")
            .RequireRateLimiting("FixedRateLimitingPolicy");

        identityApi.MapGet("claims", async (
            IIdentityService identityService, 
            ITokenAcquisition tokenAcquisition, 
            IConfiguration configuration) =>
        {
            var claims = identityService.GetUser()?.Claims;

            var dictionary = claims?.ToDictionary(
                x => x.Type,
                x => x.Value,
                StringComparer.OrdinalIgnoreCase)!;

            var ordering_scope = configuration.GetValue<string>("AzureScopes:ordering_access")!;
            var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync([ordering_scope]);
            dictionary["access_token"] = accessToken;

            return Results.Ok(dictionary);
        });

        var basketApi = app.MapGroup("/api/basket");

        basketApi.MapGet("", Basket.GetByUser.Handle)
            .WithName("GetBasketByUser")
            .WithSummary("Get the current user's basket.")
            .WithDescription("Get the current user's basket.")
            .WithTags("Basket")
            .RequireAuthorization()
            .RequireRateLimiting("FixedRateLimitingPolicy");

        return app;
    }
}


public class ClaimDto(string Type, string Value);
