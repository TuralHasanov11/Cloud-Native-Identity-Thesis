namespace WebApp.Bff.Features.Identity;

public sealed record UserInfoDto(string? Id, string? Name)
{
    public static UserInfoDto Guest => new(string.Empty, string.Empty);
}
