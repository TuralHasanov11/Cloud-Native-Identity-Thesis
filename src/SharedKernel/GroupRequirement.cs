using Microsoft.AspNetCore.Authorization;

namespace SharedKernel;

public sealed class GroupRequirement : IAuthorizationRequirement
{
    public string[] Groups { get; }

    public GroupRequirement(params string[] groups)
    {
        ArgumentNullException.ThrowIfNull(groups, nameof(groups));
        Groups = groups;
    }
}

public sealed class GroupHandler : AuthorizationHandler<GroupRequirement>
{
    public const string ClaimType = "cognito:groups";

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRequirement requirement)
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
