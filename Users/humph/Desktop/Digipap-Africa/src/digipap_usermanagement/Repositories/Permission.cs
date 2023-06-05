using Digipap.Models.Identity.DbEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Digipap.Repositories.Authorization;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler() { }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null)
        {
            return Task.FromException(new Exception("User is null!"));
        }
        var permissionss = context.User.Claims.Where(x => x.Type == "Permission" && x.Value == requirement.Permission && x.Issuer == "Digipapafrica.com");
        if (permissionss.Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        else
        {
            return Task.FromException(new Exception("Could not process request"));
        }
    }
}

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy>? GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult(policy.Build());
        }
        return FallbackPolicyProvider.GetPolicyAsync(policyName)!;
    }
    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
}