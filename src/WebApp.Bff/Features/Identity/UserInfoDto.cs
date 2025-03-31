namespace WebApp.Bff.Features.Identity;

public sealed record UserInfoDto(string? Id)
{
    public static UserInfoDto Guest => new(string.Empty);
}
