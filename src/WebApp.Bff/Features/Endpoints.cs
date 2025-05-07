using Microsoft.AspNetCore.Authentication;
using ServiceDefaults.Identity;

namespace WebApp.Bff.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapBffApi(this IEndpointRouteBuilder app)
    {
        var identityRoutes = app.MapGroup("/identity");
        identityRoutes.MapGet("login", (string? returnUrl) =>
        {
            var authenticationProperties = GetAuthenticationProperties(returnUrl);

            return TypedResults.Challenge(authenticationProperties);
        })
            .AllowAnonymous();

        var identityApi = app.MapGroup("/api/identity");

        identityApi.MapGet("user-info", Identity.UserInfo.Handle)
            .WithName("UserInfo")
            .WithSummary("Get the current user's information.")
            .WithDescription("Get the current user's information.")
            .WithTags("Identity")
            .RequireAuthorization()
            .RequireRateLimiting("FixedRateLimitingPolicy");

        //identityApi.MapGet("claims/azure", async (
        //    IIdentityService identityService, 
        //    ITokenAcquisition tokenAcquisition, 
        //    IConfiguration configuration) =>
        //{
        //    var claims = identityService.GetUser()?.Claims;

        //    var dictionary = claims?.ToDictionary(
        //        x => x.Type,
        //        x => x.Value,
        //        StringComparer.OrdinalIgnoreCase)!;

        //    var ordering_scope = configuration.GetValue<string>("AzureScopes:ordering_access")!;
        //    var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync([ordering_scope]);
        //    dictionary["access_token"] = accessToken;

        //    return Results.Ok(dictionary);
        //});

        identityApi.MapGet("claims/aws", async (
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService,
            IConfiguration configuration) =>
        {
            var claims = identityService.GetUser()?.Claims;

            var dictionary = claims?.ToDictionary(
                x => x.Type,
                x => x.Value,
                StringComparer.OrdinalIgnoreCase)!;

            string? accessToken = await httpContextAccessor.HttpContext?.GetTokenAsync("access_token");

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

    private static AuthenticationProperties GetAuthenticationProperties(string? returnUrl)
    {
        const string pathBase = "/";

        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = new Uri(pathBase, UriKind.Absolute).AbsoluteUri;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).AbsoluteUri;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}


public class ClaimDto(string Type, string Value);
