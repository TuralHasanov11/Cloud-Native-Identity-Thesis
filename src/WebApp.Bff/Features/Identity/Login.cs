namespace WebApp.Bff.Features.Identity;

public static class Login
{
    public static IResult Handle(string returnUrl = "/")
    {
        return Results.Redirect(returnUrl);
    }
}
