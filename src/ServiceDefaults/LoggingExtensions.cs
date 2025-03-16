using Microsoft.AspNetCore.Builder;
using Serilog;

namespace ServiceDefaults;

public static class LoggingExtensions
{
    public static WebApplication UseDefaultLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}
