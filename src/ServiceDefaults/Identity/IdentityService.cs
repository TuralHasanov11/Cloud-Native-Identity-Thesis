using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ServiceDefaults.Identity;

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public ClaimsPrincipal? GetUser()
    {
        return context.HttpContext?.User;
    }
}
