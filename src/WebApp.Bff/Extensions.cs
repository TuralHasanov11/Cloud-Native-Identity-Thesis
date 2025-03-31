namespace WebApp.Bff;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder _)
    {

    }

    private static void AddAIServices(this IHostApplicationBuilder _)
    {

    }
}
