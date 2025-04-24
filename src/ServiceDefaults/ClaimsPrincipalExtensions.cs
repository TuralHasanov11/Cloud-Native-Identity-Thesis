using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public static string? GetUserName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(JwtRegisteredClaimNames.Name);

    public static Dictionary<string, string> GetAddress(this ClaimsPrincipal principal)
    {
        var address = principal.FindFirst(JwtRegisteredClaimNames.Address);

        if (address == null)
        {
            return [];
        }

        var addressData = address.Value.Split(';');

        return new Dictionary<string, string>
        {
            { "Street", addressData[0] },
            { "City", addressData[1] },
            { "State", addressData[2] },
            { "Country", addressData[3] },
            { "ZipCode", addressData[4] }
        };
    }

    //public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
    //{
    //    return [.. principal.FindAll(JwtClaimTypes.Role).Select(x => x.Value)];
    //}
}
