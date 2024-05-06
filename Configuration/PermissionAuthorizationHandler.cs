using Microsoft.AspNetCore.Authorization;
using ptdn_net.Common;
using ptdn_net.Services.System.Interfaces;
using ptdn_net.Utils;

namespace ptdn_net.Configuration;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ICacheService _cacheService;

    public PermissionAuthorizationHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            context.Fail();
            return;
        }
        
        var currUsername = SessionUtil.GetCurrUsername();
        var currUser = await _cacheService.GetUser(currUsername);

        if (!currUser.IsActive)
        {
            context.Fail();
            return;
        }
        
        if (currUser.Username != "admin")
        {
            if (!currUser.Role!.IsActive)
            {
                context.Fail();
                return;
            }

            var hasPermission = currUser.Role!.Permissions.Any(x => x.Code.Contains(requirement.Permission));
            if (!hasPermission)
            {
                context.Fail();
                return;
            }
        }

        context.Succeed(requirement);
    }
}