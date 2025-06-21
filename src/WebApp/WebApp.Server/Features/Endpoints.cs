using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using ServiceDefaults.Identity;

namespace WebApp.Server.Features;

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
            .RequireRateLimiting("FixedRateLimitingPolicy");

        identityApi.MapGet("claims/azure", async (
            IIdentityService identityService,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration) =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            ITokenAcquisition? tokenAcquisition = scope.ServiceProvider.GetService<ITokenAcquisition>();

            var claims = identityService.GetUser()?.Claims;

            var dictionary = claims?.ToDictionary(
                x => x.Type,
                x => x.Value,
                StringComparer.OrdinalIgnoreCase)!;

            var scopes = configuration[$"{IdentityProviderSettings.AzureAd}:Scopes"]
                ?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? [];

            if (tokenAcquisition is not null)
            {
                var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                dictionary["access_token"] = accessToken;
            }

            return Results.Ok(dictionary);
        });

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


        identityApi.MapGet("claims/gcp", async (
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

        basketApi.MapPost("", Basket.UpdateByUser.Handle)
            .WithName("UpdateBasketByUser")
            .WithSummary("Update the current user's basket.")
            .WithDescription("Update the current user's basket.")
            .WithTags("Basket")
            .RequireAuthorization()
            .RequireRateLimiting("FixedRateLimitingPolicy");

        basketApi.MapDelete("", Basket.DeleteByUser.Handle)
            .WithName("DeleteBasketByUser")
            .WithSummary("Delete the current user's basket.")
            .WithDescription("Delete the current user's basket.")
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
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).AbsolutePath;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}


public class ClaimDto(string Type, string Value);
