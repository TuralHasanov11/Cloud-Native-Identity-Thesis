using System.Security.Claims;

namespace ServiceDefaults.Identity;

public interface IIdentityService
{
    ClaimsPrincipal? GetUser();
}
