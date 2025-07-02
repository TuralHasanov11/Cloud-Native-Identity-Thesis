using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;

namespace ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal.FindFirstValue("oid") ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public static string? GetUserName(this ClaimsPrincipal principal) 
        => principal.FindFirstValue("cognito:username") ?? principal.FindFirstValue(JwtRegisteredClaimNames.Name);

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

    public static IEnumerable<string> EntraIdAppRoles(this ClaimsPrincipal principal)
    {
        return [.. principal.FindAll("roles").Select(x => x.Value)];
    }

    public static IEnumerable<string> CognitoGroups(this ClaimsPrincipal principal)
    {
        return [.. principal.FindAll("cognito:groups").Select(x => x.Value)];
    }
}
