using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;

namespace Basket.Api.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    public static string? GetUserId(this ServerCallContext context)
    {
        var claim = context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return claim ?? "66d89b0b-eaae-4853-90c3-238d4531bd1a";
    }

    public static string? GetUserName(this ServerCallContext context)
        => context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Name);
}
