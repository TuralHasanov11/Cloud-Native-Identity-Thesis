using System.Text;

namespace Webhooks.IntegrationTests;

public static class IdentityExtensions
{
    private static readonly Random _random = new();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateId()
    {
        var stringBuilder = new StringBuilder(10);
        for (int i = 0; i < 10; i++)
        {
            stringBuilder.Append(_chars[_random.Next(_chars.Length)]);
        }

        return stringBuilder.ToString();
    }
}
