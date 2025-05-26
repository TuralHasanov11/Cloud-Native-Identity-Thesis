using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;

namespace Basket.Api.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    public static string? GetUserId(this ServerCallContext context)
    {
        return context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Sub);
    }

    public static string? GetUserName(this ServerCallContext context)
        => context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Name);
}
