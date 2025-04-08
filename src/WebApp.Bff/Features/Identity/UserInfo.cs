using Microsoft.AspNetCore.Http.HttpResults;
using ServiceDefaults.Identity;

namespace WebApp.Bff.Features.Identity;

public static class UserInfo
{
    public static async Task<Results<Ok<UserInfoDto>, UnauthorizedHttpResult>> Handle(
        IIdentityService identityService)
    {
        var user = identityService.GetUser();

        if (user is null)
        {
            return TypedResults.Ok(UserInfoDto.Guest);
        }

        var userInfo = new UserInfoDto(user.GetUserId(), user.GetUserName());

        return TypedResults.Ok(userInfo);
    }
}
