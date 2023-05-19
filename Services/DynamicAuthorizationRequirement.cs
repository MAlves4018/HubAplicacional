using Microsoft.AspNetCore.Authorization;


namespace WebApp.Services
{
    public class DynamicAuthorizationRequirement : IAuthorizationRequirement
    {
        public DynamicAuthorizationRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}
