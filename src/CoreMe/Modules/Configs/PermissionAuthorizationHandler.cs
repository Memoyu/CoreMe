using CoreMe.Service.Core.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreMe.Modules.Configs
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionAuthorizationHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            Claim userIdClaim = context.User?.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;
                if (await _permissionService.CheckAsync(requirement.Name, long.Parse(userId)))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
