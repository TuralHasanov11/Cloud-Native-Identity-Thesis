namespace WebApp.Server.Features.Identity;

public sealed record UserInfoDto(string? Id, string? Name, Address Address)
{
    public static UserInfoDto Guest => new(string.Empty, string.Empty, Address.Empty);
}


public readonly record struct Address(string Street, string City, string State, string Country, string ZipCode)
{
    public static Address Empty => new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
}

public static class AddressExtensions
{
    public static Address ToAddress(Dictionary<string, string> address)
    {
        return new Address(
            address.TryGetValue("Street", out var street) ? street : string.Empty,
            address.TryGetValue("City", out var city) ? city : string.Empty,
            address.TryGetValue("State", out var state) ? state : string.Empty,
            address.TryGetValue("Country", out var country) ? country : string.Empty,
            address.TryGetValue("ZipCode", out var zipCode) ? zipCode : string.Empty);
    }
}
