using ServiceDefaults.Identity;
using Yarp.ReverseProxy.Transforms;

namespace WebApp.Bff.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.Services.AddReverseProxy()
           .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
           .AddTransforms(context =>
           {
               context.AddRequestTransform(async request =>
               {
                   //var token = await request.HttpContext.GetUserAccessTokenAsync();
                   //request.ProxyRequest.Headers.Authorization =
                   //        new AuthenticationHeaderValue("Bearer", token.AccessToken);
               });
           })
           .AddCorrelationId();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }

    private static void AddAIServices(this IHostApplicationBuilder _)
    {

    }
}
