using Microsoft.AspNetCore.Http.HttpResults;
using ServiceDefaults.Identity;

namespace WebApp.Server.Features.Identity;

public static class UserInfo
{
    public static async Task<Results<Ok<UserInfoDto>, UnauthorizedHttpResult>> Handle(
        IIdentityService identityService)
    {
        var user = identityService.GetUser();

        if (user is not null && user.Identity?.IsAuthenticated == true)
        {
            var userInfo = new UserInfoDto(user.GetUserId(), user.GetUserName(), AddressExtensions.ToAddress(user.GetAddress()));

            return TypedResults.Ok(userInfo);
        }

        return TypedResults.Ok(UserInfoDto.Guest);
    }
}
