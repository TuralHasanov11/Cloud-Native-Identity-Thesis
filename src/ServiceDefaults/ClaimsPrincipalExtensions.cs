using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(JwtRegisteredClaimNames.Name);

    //public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
    //{
    //    return [.. principal.FindAll(JwtClaimTypes.Role).Select(x => x.Value)];
    //}
}
