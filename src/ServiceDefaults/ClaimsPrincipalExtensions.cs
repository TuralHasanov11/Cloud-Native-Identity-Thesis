using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.FindFirst(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;

    //public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
    //{
    //    return [.. principal.FindAll(JwtClaimTypes.Role).Select(x => x.Value)];
    //}
}
