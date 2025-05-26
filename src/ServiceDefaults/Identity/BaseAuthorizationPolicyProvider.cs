using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ServiceDefaults.Identity;

public class BaseAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public const string GroupPolicyPrefix = "RequireGroup";

    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public BaseAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Group Based Authorization
        if (policyName.StartsWith(GroupPolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser();

            var group = policyName[GroupPolicyPrefix.Length..];

            policy.AddRequirements(new GroupRequirementAttribute(group));
            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
