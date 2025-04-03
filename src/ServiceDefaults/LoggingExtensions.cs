using Microsoft.AspNetCore.Builder;

namespace ServiceDefaults;

public static class LoggingExtensions
{
    public static WebApplication UseDefaultLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}
