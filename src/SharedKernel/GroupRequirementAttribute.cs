using Microsoft.AspNetCore.Authorization;

namespace SharedKernel;

public sealed class GroupRequirementAttribute :
    AuthorizeAttribute,
    IAuthorizationRequirement,
    IAuthorizationRequirementData
{
    public GroupRequirementAttribute(params string[] groups)
    {
        ArgumentNullException.ThrowIfNull(groups, nameof(groups));
        Groups = groups;
    }

    public string[] Groups { get; }

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}

public sealed class GroupHandler : AuthorizationHandler<GroupRequirementAttribute>
{
    public const string ClaimType = "cognito:groups";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        GroupRequirementAttribute requirement)
    {
        foreach (var group in requirement.Groups)
        {
            if (context.User.HasClaim(c => c.Type == ClaimType && c.Value == group))
            {
                context.Succeed(requirement);
                break;
            }
        }

        return Task.CompletedTask;
    }
}
