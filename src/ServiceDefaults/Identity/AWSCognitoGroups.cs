namespace ServiceDefaults.Identity;

public sealed class AWSCognitoGroups
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static IEnumerable<string> All()
    {
        yield return Admin;
        yield return Customer;
    }
}
