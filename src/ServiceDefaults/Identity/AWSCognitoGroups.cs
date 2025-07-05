namespace ServiceDefaults.Identity;

public static class AwsCognitoGroups
{
    public const string Admins = "Admins";
    public const string Customers = "Customers";

    public static IEnumerable<string> All()
    {
        yield return Admins;
        yield return Customers;
    }
}
