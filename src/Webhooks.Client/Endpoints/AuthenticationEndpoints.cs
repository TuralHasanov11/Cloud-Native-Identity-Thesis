using Microsoft.AspNetCore.Antiforgery;

namespace Webhooks.Client.Endpoints;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", async (HttpContext httpContext, IAntiforgery antiforgery) =>
        {
            //await antiforgery.ValidateRequestAsync(httpContext);
            //await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        });

        return app;
    }
}
