using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;

namespace Basket.Api.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    public static Guid GetUserId(this ServerCallContext context)
    {
        var claim = context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(claim, out var userId) ? userId : Guid.Empty;
    }

    public static string? GetUserName(this ServerCallContext context)
        => context.GetHttpContext().User.FindFirstValue(JwtRegisteredClaimNames.Name);
}
