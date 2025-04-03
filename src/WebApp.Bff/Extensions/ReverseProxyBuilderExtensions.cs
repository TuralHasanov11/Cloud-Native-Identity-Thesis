using Yarp.ReverseProxy.Transforms;

namespace WebApp.Bff.Extensions;

internal static class ReverseProxyBuilderExtensions
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
}
