﻿using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ServiceDefaults.Middleware;

public class RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) : IMiddleware
{
    private readonly ILogger<RequestTimeMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        _logger.LogRequestStarted();

        await next(context);

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
        _logger.LogRequestFinished(elapsedTime);
    }
}

internal static partial class RequestTimeMiddlewareLogger
{
    [LoggerMessage(LogLevel.Information, "Request started")]
    internal static partial void LogRequestStarted(
        this ILogger<RequestTimeMiddleware> logger);

    [LoggerMessage(LogLevel.Information, "Request finished in {ElapsedTime}")]
    internal static partial void LogRequestFinished(
        this ILogger<RequestTimeMiddleware> logger,
        TimeSpan elapsedTime);
}
