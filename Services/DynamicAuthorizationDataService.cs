using Microsoft.EntityFrameworkCore;   
using System.Security.Claims; 
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class DynamicAuthorizationDataService : IDynamicAuthorizationDataService
    {
        private readonly ApplicationDbContext _context;

        public DynamicAuthorizationDataService(ApplicationDbContext context )
        {
            _context = context;
        }


        public async Task<List<NavigationMenuNode>> GetUserMenusAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return new List<NavigationMenuNode>();
            }

            // get all roles the principal belongs to
            var roleIds = await GetUserRoleIds(principal);
 
            var menuItems = (from rid in roleIds
                join
                    v in (_context.RoleMenuPermissions).Include(x => x.NavigationMenu) on rid equals v.RoleId
                select v.NavigationMenu).Distinct().ToList();

            var treeRoot = ModelFactory.AsNavigationMenuNodeList(menuItems, null);
            return treeRoot;
        }
        

        public async Task<bool> IsAdmin(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);

            var roleIds = await (from role in _context.UserRoles
                where role.UserId == userId
                join s in _context.Roles on role.RoleId equals s.Id
                select s.Name).ToListAsync();
            
            return roleIds.Contains("Admin");
        }

        public async Task<bool> IsAccessAuthorized(ClaimsPrincipal ctx, string area, string ctrl, string act, bool isRead)
        {
            var result = false;
            var roleIds = await GetUserRoleIds(ctx);
            
            var data = await (from perm in _context.RoleMenuPermissions.Include("NavigationMenu")
                    where roleIds.Contains(perm.RoleId)
                    select perm)
                .Select(m => new {  m.NavigationMenu.ControllerName, 
                    m.NavigationMenu.ActionName, m.Get, m.Post})
                .Distinct()
                .ToListAsync();

            bool IsSame(string a, string b)
            {
                return string.IsNullOrEmpty(a) ? string.IsNullOrEmpty(b) : a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
            }
            
            foreach (var item in data)
            {
                result = ( IsSame(item.ControllerName, ctrl) && 
                           IsSame( item.ActionName , act) &&
                           (isRead ? item.Get.Value : item.Post.Value)
                           );
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public async Task<List<RolePermission>> GetRolePermissionsAsync(string id)
        {
            try
            {
                var items = await (from m in _context.NavigationMenus
                                   orderby m.DisplayOrder
                                   join rm in _context.RoleMenuPermissions
                                       on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                                       into rmp
                                   from zz in rmp.DefaultIfEmpty()
                                   select new RolePermission()
                                   {
                                       Id = m.Id,
                                       ParentMenuId = m.ParentMenuId,
                                       Name = m.Name,
                                       Get = zz.Get.HasValue ? zz.Get : false,
                                       Post = zz.Post.HasValue ? zz.Post : false
                                   })
                    .AsNoTracking()
                    .ToListAsync();
                //var items = await (from m in _context.NavigationMenus
                //                   orderby m.DisplayOrder
                //                   join rm in _context.RoleMenuPermissions
                //                       on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                //                   select new RolePermission()
                //                   {
                //                       Id = m.Id,
                //                       ParentMenuId = m.ParentMenuId,
                //                       Name = m.Name,
                //                       Get = rm.Get?,
                //                       Post = rm.Post
                //                   })
                //    .AsNoTracking()
                //    .ToListAsync();

                return items;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public async Task<List<RoleUsersViewModel>> GetRoleMembersAsync(string roleId)
        {
//            var items = await (_context.Users.OrderBy(u=>u.UserName).Join(// outer sequence 
//                _context.UserRoles ,  // inner sequence 
//                user => new {Id=user.Id, RoleId=roleId},    // outerKeySelector
//                role => new {Id=role.UserId, RoleId=role.RoleId},  // innerKeySelector
//                (user, role) => new RoleUsersViewModel()
//                {
//                    Id = user.Id,
//                    UserName = user.UserName,
//                    Selected = true
//                })).AsNoTracking().ToListAsync();
            var items = (await _context.Users.OrderBy(u => u.UserName).ToListAsync()).Join(
                (await _context.UserRoles.Where( r => r.RoleId == roleId).ToListAsync()),
                user =>  user.Id,    // outerKeySelector
                role =>  role.UserId,  // innerKeySelector
                (user, role) => new RoleUsersViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Selected = true
                }).ToList();
            return items;
        }

        public async Task<List<RoleUsersViewModel>> GetUsersRoleMembershipAsync(string roleId)
        {
            var items = (await _context.Users.OrderBy(u => u.UserName).ToListAsync()).GroupJoin(
                (await _context.UserRoles.Where( r => r.RoleId == roleId).ToListAsync()),
                user =>  user.Id,    
                role =>  role.UserId,  
                (user, roleGroup) => new RoleUsersViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Selected = roleGroup.Any()
                }).ToList();
            
            return  items;
        }

        public async Task<List<RolePermission>> GetUserPermissionsAsync(string userId)
        {
            var param = new SqlParameter("@UserId", userId);

            var permissions = await _context.RolePermissions.FromSqlRaw("GetUserPermissions @UserId", param)
                .ToListAsync();
            return permissions;
        }

        public async Task<bool> SetRolePermissionsAsync(string id, IEnumerable<RolePermission> permissions)
        {
            await using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
            {
                try
                {
                    var existing = await _context.RoleMenuPermissions.Where(x => x.RoleId == id).ToListAsync();

                    _context.RemoveRange(existing);

                    foreach (var item in permissions)
                    {
                        await _context.RoleMenuPermissions.AddAsync(new RoleMenuPermission()
                        {
                            RoleId = id,
                            NavigationMenuId = item.Id,
                            Get = item.Get,
                            Post = item.Post
                        });
                    }

                    await _context.SaveChangesAsync();
                    dbTran.Commit();
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    return false;
                }
            }

            return true;
        }
        
        private async Task<List<string>> GetUserRoleIds(string userId)
        {
            var data = await (from role in _context.UserRoles
                where role.UserId == userId
                select role.RoleId).ToListAsync();

            return data;
        }
        
        private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            if(userId == null) 
            {
                return new List<string> { };
            } 
            else 
            {
                return await GetUserRoleIds(userId);
            }

        }

        public async Task<List<UserRolesViewModel>> GetUserRoles(string  userId)
        {
            var data = await (from role in _context.UserRoles 
                where role.UserId == userId
                join s in _context.Roles on role.RoleId equals s.Id
                select new UserRolesViewModel() {Id = s.Id, Name = s.Name, Selected = true}).OrderBy(m => m.Name).ToListAsync();

            return data;
        }
        
        
        private string GetUserId(ClaimsPrincipal user)
        {
            //return ((ClaimsIdentity) user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            try
            {
                var nim = user.Identity.Name.Split("\\")[1];
                var identityUser = _context.Users.FirstOrDefault(x => x.UserName == nim);

                return identityUser.Id.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        
        }
    }
}