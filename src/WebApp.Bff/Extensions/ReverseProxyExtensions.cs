using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace WebApp.Bff.Extensions;

internal static class ReverseProxyExtensions
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public static IReverseProxyBuilder AddCorrelationId(this IReverseProxyBuilder builder)
    {
        builder.AddTransforms(transform =>
        {
            transform.AddRequestTransform(t =>
            {
                if (t.ProxyRequest.Headers.Any(h => h.Key == CorrelationIdHeaderName))
                {
                    return ValueTask.CompletedTask;
                }

                var correlationId = Guid.CreateVersion7().ToString("N");
                t.ProxyRequest.Headers.Add(CorrelationIdHeaderName, correlationId);

                return ValueTask.CompletedTask;
            });
        });

        return builder;
    }

    public static async Task AddAccessTokenAsync(this RequestTransformContext request, TransformBuilderContext context)
    {
        ITokenAcquisition? tokenAcquisition = context.Services.GetService<ITokenAcquisition>();
        IConfiguration configuration = context.Services.GetRequiredService<IConfiguration>();

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
                accessToken = await request.HttpContext!.GetTokenAsync("access_token");

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
            request.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.ProxyRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
