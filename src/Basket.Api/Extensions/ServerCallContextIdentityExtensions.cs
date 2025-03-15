using System.IdentityModel.Tokens.Jwt;
using Grpc.Core;

namespace Basket.Api.Extensions;

internal static class ServerCallContextIdentityExtensions
{
    public static string? GetUserId(this ServerCallContext context)
        => context.GetHttpContext().User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

    public static string? GetUserName(this ServerCallContext context)
        => context.GetHttpContext().User.FindFirst(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
}
