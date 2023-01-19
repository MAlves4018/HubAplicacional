using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApp.Services
{
    public class DynamicAuthorizationHandler : AuthorizationHandler<DynamicAuthorizationRequirement>
    {
        private readonly IDynamicAuthorizationDataService _dataAccessService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DynamicAuthorizationHandler(IDynamicAuthorizationDataService dataAccessService,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataAccessService = dataAccessService;
            _httpContextAccessor = httpContextAccessor;
        }
        

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DynamicAuthorizationRequirement requirement)
        {
            var isRead = _httpContextAccessor.HttpContext.Request.Method.ToUpper() == "GET";
            object area = null;
            object controller = null;
            object action = null;

            switch (context.Resource)
            {
                case HttpContext endpoint5: /* dotnet core 5 */
                    endpoint5.Request.RouteValues.TryGetValue("area", out area);
                    endpoint5.Request.RouteValues.TryGetValue("controller", out controller);
                    endpoint5.Request.RouteValues.TryGetValue("action", out action);
                    break;
                case RouteEndpoint endpoint3: /* dotnet core 3 */
                    endpoint3.RoutePattern.RequiredValues.TryGetValue("area", out area);
                    endpoint3.RoutePattern.RequiredValues.TryGetValue("controller", out controller);
                    endpoint3.RoutePattern.RequiredValues.TryGetValue("action", out action);
                    break;
            }

            /* the user belongs to the admin group or has explicit group permissions */
            if (context.User.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(requirement?.PermissionName))
            {
                if (requirement.PermissionName.Equals(DynamicPolicies.DynamicAdmin) &&
                    await _dataAccessService.IsAdmin(context.User)
                    || DynamicPolicies.Exists(requirement.PermissionName) &&
                    await _dataAccessService.IsAccessAuthorized(
                        context.User,
                        (area == null) ? "" : area.ToString(),
                        (controller == null) ? "" : controller.ToString(),
                        (action == null) ? "" : action.ToString(),
                        isRead
                    )
                )
                    context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}