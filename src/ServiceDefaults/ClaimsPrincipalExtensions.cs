using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "66d89b0b-eaae-4853-90c3-238d4531bd1a";

    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(JwtRegisteredClaimNames.Name) ?? "Jon Snow";

    //public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
    //{
    //    return [.. principal.FindAll(JwtClaimTypes.Role).Select(x => x.Value)];
    //}
}
