namespace ServiceDefaults.Identity;

public sealed class AWSCognitoGroups
{
    public const string Admins = "Admins";
    public const string Customers = "Customers";

    public static IEnumerable<string> All()
    {
        yield return Admins;
        yield return Customers;
    }
}
