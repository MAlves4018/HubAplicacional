using WebApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApp.Services
{
	
	public interface IDynamicAuthorizationDataService
	{

		// returns true if the principal is allowed to access the action of the controller 
		Task<bool> IsAccessAuthorized(ClaimsPrincipal ctx,  string area,  string ctrl, string act, bool isRead);
		
		Task<bool> IsAdmin(ClaimsPrincipal ctx );

		// returns the list of menu items entries the user is allowed to access
		Task<List<NavigationMenuNode>> GetUserMenusAsync(ClaimsPrincipal principal);

		// return the list of group permissions
		Task<List<RolePermission>> GetRolePermissionsAsync(string roleId);
		
		Task<bool> SetRolePermissionsAsync(string roleId, IEnumerable<RolePermission> permissions);


		Task<List<RoleUsersViewModel>> GetRoleMembersAsync(string roleId);

		Task<List<RoleUsersViewModel>> GetUsersRoleMembershipAsync(string roleId);
		
		// return the list of user permissions
		Task<List<RolePermission>> GetUserPermissionsAsync(string userId);

		Task<List<UserRolesViewModel>> GetUserRoles(string userId);
	}
}