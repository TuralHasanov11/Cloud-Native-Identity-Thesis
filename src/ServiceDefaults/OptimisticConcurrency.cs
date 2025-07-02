using Microsoft.AspNetCore.Http;

namespace ServiceDefaults;

public static class OptimisticConcurrency
{
    public static HttpContext WithETag(this HttpContext httpContext, Func<string> tagSelector)
    {
        httpContext.Response.Headers.ETag = tagSelector();
        return httpContext;
    }

    public static bool PassesConcurrencyCheck(this HttpContext httpContext, Func<string> tagSelector)
    {
        var requestETag = httpContext.Request.Headers.IfMatch.FirstOrDefault()?.Trim('"');
        return tagSelector() == requestETag;
    }
}
