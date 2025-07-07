using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace WebApp.Server.Extensions;

public class CorrelationIdTransformProvider : ITransformProvider
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(t =>
        {
            if (t.ProxyRequest.Headers.Any(h => h.Key == CorrelationIdHeaderName))
            {
                return ValueTask.CompletedTask;
            }

            var correlationId = Guid.CreateVersion7().ToString("N");
            t.ProxyRequest.Headers.Add(CorrelationIdHeaderName, correlationId);

            return ValueTask.CompletedTask;
        });
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }
}

public class JwtTransformProvider : ITransformProvider
{
    public void Apply(TransformBuilderContext context)
    {
        ITokenAcquisition? tokenAcquisition = context.Services.GetService<ITokenAcquisition>();
        IConfiguration configuration = context.Services.GetRequiredService<IConfiguration>();

        context.AddRequestTransform(async transformContext =>
        {
            string accessToken;

            // Azure
            if (tokenAcquisition is not null)
            {
                var scopes = configuration[$"{IdentityProviderSettings.AzureAd}:Scopes"]?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? [];

                try
                {
                    accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"No access token: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"No access token: {ex.Message}");
                    return;
                }
            }

            Debug.WriteLine($"access token: {accessToken}");

            if (accessToken is not null)
            {
                transformContext.ProxyRequest.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);

                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                transformContext.ProxyRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        });
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }
}
