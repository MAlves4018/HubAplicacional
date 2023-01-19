using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApp.Data;
using Microsoft.EntityFrameworkCore.Storage; 
using WebApp.Models.ApplicationModels;
using System.Data;
using System.Reflection;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDynamicAuthorizationDataService _dataAccessService;
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IUserMessages _userMessages;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IDynamicAuthorizationDataService dataAccessService,
            ILogger<AdminController> logger,
            IUserMessages userMessages
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataAccessService = dataAccessService;
            _dataAccessService = dataAccessService;
            _logger = logger;
            _context = context;
            _userMessages = userMessages;
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> Roles()
        {
            var roleViewModel = new List<RoleViewModel>();

            try
            {
                var roles = await _roleManager.Roles.OrderBy(x => x.Name).ToListAsync();
               
                foreach (var item in roles)
                {
                    roleViewModel.Add(new RoleViewModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.GetBaseException().Message);
            }

            return View(roleViewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> ViewRole(string id, bool p)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return RedirectToAction(nameof(Roles));

            var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};

            var permissions = await _dataAccessService.GetRolePermissionsAsync(id);

            var sortedPermissions = ModelFactory.AsRolePermissionListSorted(permissions, null);
            viewModel.Permissions = sortedPermissions.ToArray();

            viewModel.Members = await _dataAccessService.GetRoleMembersAsync(id);
            ViewData["useLayout"] = p;
            return View(viewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRolePermissions(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return RedirectToAction(nameof(Roles));

            var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};

            var permissions = await _dataAccessService.GetRolePermissionsAsync(id);
            var sortedPermissions = ModelFactory.AsRolePermissionListSorted(permissions, null);
            viewModel.Permissions = sortedPermissions.ToArray();

            return View(viewModel);
        }
        

        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRoleMembers(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return RedirectToAction(nameof(EditRole));

            var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};
            
            viewModel.Members = await _dataAccessService.GetUsersRoleMembershipAsync(id);
            ViewData["Title"] = "Editar Utilizadores do Perfil";
            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRoleMembers(RoleViewModel viewModel)
        {
            try
            {
                bool foundError = false;
                foreach (var roleView in viewModel.Members)
                {
                    var user = await _userManager.FindByIdAsync(roleView.Id);
                    if( user == null) continue;

                    if (roleView.Selected)
                    {
                        await _userManager.AddToRoleAsync(user, viewModel.Name);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, viewModel.Name);
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (! result.Succeeded)
                    {
                        foundError = true;
                        _userMessages.AddUserMessage("Editar Membros",
                            "Não foi possível alterar o utilizador <strong>" + user.UserName +"</strong>" , IUserMessages.ErrorCode.DANGER, 3000);   
                    }
                }
                if(!foundError)
                    _userMessages.AddUserMessage("Editar permissões",
                        "As alterações foram guardadas com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);

            }
            catch (Exception )
            {
                _userMessages.AddUserMessage("Editar Membros",
                    "Não foi possível guardar as alterações" , IUserMessages.ErrorCode.DANGER, 3000);   

            }

            return RedirectToAction(nameof(EditRoleMembers));
        }
        

        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRolePermissions(RoleViewModel viewModel)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(viewModel.Id);
                if (null == role)
                    return RedirectToAction(nameof(Roles));

                var permissions = viewModel.Permissions.Where(x => (x.Get.Value || x.Post.Value));

                if (await _dataAccessService.SetRolePermissionsAsync(viewModel.Id, permissions))
                {
                    _userMessages.AddUserMessage("Editar permissões",
                        "As alterações foram guardadas com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                    return RedirectToAction(nameof(EditRolePermissions));
                }
            }
            catch (Exception e)
            {
            }

            _userMessages.AddUserMessage("Editar permissões",
                "Não foi possível guardar as alterações.", IUserMessages.ErrorCode.DANGER, 3000);

            ViewData["Title"] = "Editar Permissões de Perfil";
            return View(viewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return RedirectToAction(nameof(Roles));

            var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};

            ViewData["Title"] = "Editar Perfil";
            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditRole(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(viewModel.Id);
                role.Name = viewModel.Name;

                if ((await _roleManager.UpdateAsync(role)).Succeeded  )
                {
                    _userMessages.AddUserMessage("Editar Perfil",
                        "As alterações foram guardadas com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                    return RedirectToAction(nameof(EditRole));
                }

                ModelState.AddModelError("Name",
                    "Não foi possível guardar as alterações.\nVerifique se esse nome já existe.");

//                    _userMessages.AddUserMessage("Editar permissões",
//                        "Não foi possível guardar as alterações.", IUserMessages.ErrorCode.DANGER, 6000);
            }

            ViewData["Title"] = "Editar Perfil";
            return View(viewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> CreateRole()
        {
            var adminRoles = _roleManager.Roles; 
            var adminRole = await _roleManager.FindByNameAsync("Admin"); 

            var viewModel = new RoleViewModel();
            var permissions = await _dataAccessService.GetRolePermissionsAsync(adminRole.Id);
            var sortedPermissions = ModelFactory.AsRolePermissionListSorted(permissions, null);
            viewModel.Permissions = sortedPermissions.ToArray();
            ViewData["Title"] = "Criar Novo Perfil";
            return View(nameof(EditRole), viewModel);
        }


        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> CreateRole(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole() {Name = viewModel.Name};
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded )
                {
                    _userMessages.AddUserMessage("Criar Perfil",
                        "O Perfil foi criado com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                    return RedirectToAction(nameof(EditRole), new { Id=role.Id});
                }

                ModelState.AddModelError("Name",
                    "Não foi possível criar o Perfil pretendido.\nVerifique se já existe um Perfil com esse nome.");
            }

            ViewData["Title"] = "Criar Novo Perfil";
            return View(nameof(EditRole), viewModel);
        }
        

        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return RedirectToAction(nameof(Roles));

            var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};

            ViewData["Title"] = "Apagar Perfil";
            return View(viewModel);
        }
        

        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> DeleteRole(RoleViewModel viewModel)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(viewModel.Id);

                if ( role != null && (await _roleManager.DeleteAsync(role)).Succeeded)
                {
                    
                    _userMessages.AddUserMessage("Apagar Perfil",
                        "O Perfil foi apagado com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                    return RedirectToAction(nameof(Roles));
                }

                _userMessages.AddUserMessage("Apagar Perfil",
                    "Não foi possível apagar o Perfil indicado.", IUserMessages.ErrorCode.DANGER, 3000);
            }
            catch (Exception)
            {
                _userMessages.AddUserMessage("Apagar Perfil",
                    "Não foi possível apagar o Perfil indicado.", IUserMessages.ErrorCode.DANGER, 3000);
            }
            
            return RedirectToAction(nameof(Roles));
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderBy(x => x.UserName).Select(
                item => new UserViewModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    UserName = item.UserName,
                    PhoneNumber = item.PhoneNumber
                }
            ).ToListAsync();

            
            foreach (var user in users)
            {
                user.Roles = (await _dataAccessService.GetUserRoles(user.Id)).ToArray();
            }

            return View(users);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> ViewUser(string Id, bool p)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
                return RedirectToAction(nameof(Users));

            var viewModel = new UserViewModel()
            {
                Id = Id,
                Email = user?.Email,
                UserName = user?.UserName,
                PhoneNumber = user?.PhoneNumber
            };

            viewModel.Roles = (await _dataAccessService.GetUserRoles(Id)).ToArray();
           
            
             

            var permissions = await _dataAccessService.GetRolePermissionsAsync(Id); 
            var sortedPermissions = ModelFactory.AsRolePermissionListSorted(permissions, null);
            viewModel.Permissions = sortedPermissions.ToArray();
             

             
            ViewData["useLayout"] = p;
            return View(viewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditUser(string Id)
        {
            try
            {
                //var user = await _userManager.FindByIdAsync(Id);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (user is null)
                    return RedirectToAction(nameof(Users));

                var viewModel = new UserViewModel();

                var userRoles = await _userManager.GetRolesAsync(user);

                viewModel.Id = Id;
                viewModel.Email = user?.Email;
                viewModel.UserName = user?.UserName;
                viewModel.PhoneNumber = user?.PhoneNumber; 

                ViewData["Title"] = "Editar utilizador";

                return View(viewModel);
            }
            catch (Exception e)
            {
                _userMessages.AddUserMessage("Editar perfil do Utilizador",
                    "As alterações solicitadas falharam", IUserMessages.ErrorCode.DANGER, 2500);
            }
            return RedirectToAction(nameof(Users));

        }

        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditUser(UserViewModel viewModel, ICollection<int> EntidadesSetoriais)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.Where(w => w.Id == viewModel.Id).FirstOrDefaultAsync();
                    if (user is null)
                        return RedirectToAction(nameof(Users));
                    var ad_user = ExercitoAD.GetInfoUserAD(viewModel.UserName);

                  
                    user.UserName = viewModel.UserName;
                    user.Email = ad_user.email;
                    user.PhoneNumber = viewModel.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _userMessages.AddUserMessage("Editar perfil do Utilizador",
                            "As alterações foram guardadas com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);

                        return RedirectToAction(nameof(EditUser));
                    }
                }
            }
            catch (Exception e)
            {
    
            }
            
            _userMessages.AddUserMessage("Editar perfil do Utilizador",
                "As alterações solicitadas falharam", IUserMessages.ErrorCode.DANGER, 2500);

            ViewData["Title"] = "Editar utilizador";
            return View(viewModel);
        }

        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditUserMembership(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user is null)
                    return RedirectToAction(nameof(Users));

                var viewModel = new UserViewModel
                {
                    Id = Id, Email = user?.Email, UserName = user?.UserName, PhoneNumber = user?.PhoneNumber
                };

                var userRoles = await _userManager.GetRolesAsync(user);
 
                var allRoles = await _roleManager.Roles.ToListAsync();
                viewModel.Roles = allRoles.Select(x => new UserRolesViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Selected = userRoles.Contains(x.Name)
                }).ToArray();

                ViewData["Title"] = "Editar utilizador";

                return View(viewModel);
            }
            catch (Exception e)
            {
                _userMessages.AddUserMessage("Editar perfis do Utilizador",
                    "As alterações solicitadas falharam", IUserMessages.ErrorCode.DANGER, 2500);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> EditUserMembership(UserViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);
                if (user is null)
                    return RedirectToAction(nameof(Users));

                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user,
                    viewModel.Roles.Where(x => x.Selected).Select(x => x.Name));

                _userMessages.AddUserMessage("Editar perfis do Utilizador",
                    "As alterações foram guardadas com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);

                return RedirectToAction(nameof(EditUserMembership));
                
            }
            catch (Exception e)
            {
            }

            _userMessages.AddUserMessage("Editar perfis do Utilizador",
                "As alterações solicitadas falharam", IUserMessages.ErrorCode.DANGER, 2500);

            ViewData["Title"] = "Editar utilizador";
            return View(viewModel);
        }
        
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> CreateUser()
        {
             var viewModel = new UserViewModel();

            var allRoles = await _roleManager.Roles.ToListAsync();
            viewModel.Roles = allRoles.Select(x => new UserRolesViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Selected = false
            }).ToArray();

            ViewData["Title"] = "Criar novo utilizador";
            return View(nameof(EditUser), viewModel);
        }

        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> CreateUser(UserViewModel viewModel, ICollection<int> EntidadesSetoriais)
        {
            if (ModelState.IsValid)
            {
                 var ad_user = ExercitoAD.GetInfoUserAD(viewModel.UserName);
                if(ad_user != null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = viewModel.UserName,
                        PhoneNumber = viewModel.PhoneNumber,
                        Email = ad_user.email, 
                    };

                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        _userMessages.AddUserMessage("Criar utilizador",
                            "O utilizador " + user.UserName + " foi criado com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                        return RedirectToAction(nameof(Users));
                    }

                    _userMessages.AddUserMessage("Criar utilizador",
                            "Não foi possível criar o utilizador. Verifique se já existe.", IUserMessages.ErrorCode.DANGER, 2500);
                    ModelState.AddModelError("Roles", "Não foi possível criar o utilizador. Verifique se já existe.");
                }
                else
                {
                    _userMessages.AddUserMessage("Criar utilizador",
                            "O utilizador " + viewModel.UserName + " não existe na Active Directory do Exército.", IUserMessages.ErrorCode.DANGER, 2500);

                }

            }

            ViewData["Title"] = "Criar novo utilizador";
            return View(nameof(EditUser), viewModel);
        }

        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
                return RedirectToAction(nameof(Users));

            var viewModel = new UserViewModel
            {
                Id = Id, Email = user?.Email, UserName = user?.UserName, PhoneNumber = user?.PhoneNumber
            };


            ViewData["Title"] = "Apagar utilizador";
            return View(viewModel);
        }
        
        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> DeleteUser(UserViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);

                if ( user != null && (await _userManager.DeleteAsync(user)).Succeeded)
                {
                    _userMessages.AddUserMessage("Apagar utilizador",
                        "O utilizador foi apagado com sucesso.", IUserMessages.ErrorCode.SUCCESS, 2500);
                    return RedirectToAction(nameof(Users));
                }

                _userMessages.AddUserMessage("Apagar utilizador",
                    "Não foi possível apagar o utilizador indicado.", IUserMessages.ErrorCode.DANGER, 3000);
            }
            catch (Exception)
            {
                _userMessages.AddUserMessage("Apagar utilizador",
                    "Não foi possível apagar o utilizador indicado.", IUserMessages.ErrorCode.DANGER, 3000);
            }
            
            return RedirectToAction(nameof(Users));
        }
        
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> ViewUserPermissions(string id)
        {
            //var viewModel = new RoleViewModel() {Id = role.Id, Name = role.Name};

            var permissions = await _dataAccessService.GetUserPermissionsAsync(id);
            if (!permissions.Any())
                return RedirectToAction(nameof(Users));


            var sortedPermissions = ModelFactory.AsRolePermissionListSorted(permissions, null);

            return View(sortedPermissions);
        }


        //[Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        //public async Task<IActionResult> Menus()
        //{
        //    //var permissions = await _context.NavigationMenus.Include(c => c.ParentNavigationMenu).ToListAsync();
        //    var permissions = await _context.NavigationMenus.ToListAsync();

        //    var treeRoot = ModelFactory.AsNavigationMenuNodeList(permissions, null);

        //    var json = JsonConvert.SerializeObject(treeRoot);
        //    var viewModel = new NavigationMenusViewModel() { Menus = json };

        //    _userMessages.AddUserMessageOnce("admin/permissions/1", "Ajuda",
        //        "Ordene os elementos arrastando o botão &emsp;<i class='bi bi-arrows-move text-warning'></i><br/><br/>Para editar faça duplo clique sobre o elemento pretendido",
        //        IUserMessages.ErrorCode.INFO, 12000);

        //    return View(viewModel);
        //}


        [HttpPost]
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Menus(NavigationMenusViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                List<NavigationMenuNode> nodes =
                    JsonConvert.DeserializeObject<List<NavigationMenuNode>>(viewModel.Menus);

                var itemList = ModelFactory.AsNavigationMenuList(nodes);
                var itemListIds = itemList.Select(x => x.Id);

                var currListIds = await _context.NavigationMenus.Select(x => x.Id).ToListAsync();
                var toRemoveIds = currListIds.Where(x => !itemListIds.Contains(x)).ToList();
                var toAddIds = itemListIds.Where(x => !currListIds.Contains(x)).ToList();

                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        /*
                        mudar todos os registos para a raiz (pai NULL)
                       depois apagar antes de adicionar
                        */
                        _context.Database.ExecuteSqlRaw($"UPDATE [AspNetNavigationMenus] SET ParentMenuId=NULL");

                        await _context.SaveChangesAsync(User.Identity.Name);

                        foreach (var itn in itemList)
                        {
                            try
                            {
                                _context.NavigationMenus.Update(itn);
                                await _context.SaveChangesAsync(User.Identity.Name);
                            }
                            catch (Exception a)
                            {
                                _context.NavigationMenus.Add(itn);
                                await _context.SaveChangesAsync(User.Identity.Name);
                            }
                        }

                        //                        foreach (var itnId in toRemoveIds)
                        //                        {
                        //                            _context.Database.ExecuteSqlRaw(
                        //                                $"UPDATE [AspNetNavigationMenus] SET ParentMenuId=NULL  WHERE ID='{itnId}'");
                        //                            await _context.SaveChangesAsync(User.Identity.Name);
                        //                        }

                        foreach (var itnId in toRemoveIds)
                        {
                            _context.Database.ExecuteSqlRaw($"DELETE FROM [AspNetNavigationMenus] WHERE ID='{itnId}'");
                            await _context.SaveChangesAsync(User.Identity.Name);
                        }


                        dbTran.Commit();
                    }
                    catch (Exception e)
                    {
                        dbTran.Rollback();
                        _userMessages.AddUserMessage("Editar permissões",
                            "Não foi possível guardar as alterações.", IUserMessages.ErrorCode.DANGER, 6000);
                        return View(viewModel);
                    }
                }

                _userMessages.AddUserMessage("Editar permissões",
                    "As alterações foram guardadas com sucesso.",
                    IUserMessages.ErrorCode.SUCCESS, 7000);

                if (toAddIds.Any())
                {
                    _userMessages.AddUserMessage("Ajuda",
                        "Foram adicionadas " + toAddIds.Count + " novos elementos.<br/>"
                        + "O acesso às novas permissões pode ser configurado na<a class=\"text-warning\" href=\"./"
                        + nameof(Roles) + "\"> página de perfis.</a>",
                        IUserMessages.ErrorCode.INFO, 12000);
                }
                else
                {
                    _userMessages.AddUserMessageOnce("admin/permissions/2", "Ajuda",
                        "O acesso às novas permissões pode ser configurado na<a class=\"text-warning\" href=\"./"
                        + nameof(Roles) + "\"> página de perfis.</a>",
                        IUserMessages.ErrorCode.INFO, 12000);
                }

                return RedirectToAction(nameof(Menus));
            }
            else
            {
                _userMessages.AddUserMessage("Editar permissões",
                    "Não foi possível guardar as alterações.", IUserMessages.ErrorCode.DANGER, 6000);
            }

            return View(viewModel);
        }


        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> ViewMenu(string id)
        {
            var x = await _context.NavigationMenus.Where(a => a.Id == new Guid(id)).FirstOrDefaultAsync();

            var viewModel = ModelFactory.AsNavigationMenuModel(x);

            return View(viewModel);
        }




        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IActionResult> Menus()
        {
            var listMenus = await GetControllers(); 
            var treeRoot = ModelFactory.AsNavigationMenuNodeList(listMenus, null);

            var json = JsonConvert.SerializeObject(treeRoot);
            var viewModel = new NavigationMenusViewModel() { Menus = json };

            _userMessages.AddUserMessageOnce("admin/permissions/1", "Ajuda",
                "Ordene os elementos arrastando o botão &emsp;<i class='bi bi-arrows-move text-warning'></i><br/><br/>Para editar faça duplo clique sobre o elemento pretendido",
                IUserMessages.ErrorCode.INFO, 12000);

            return View(viewModel); 
        }



         
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public List<IGrouping<string, ControllerAction>> GetControllersFromAssembly()
        {
            //Controladores da Aplicação
            var asm = Assembly.GetAssembly(typeof(AdminController));
            var controlleractionlist = asm.GetTypes()
                    //.Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Where(x => x.DeclaringType.Name.EndsWith("Controller"))
                    .Select(x => new ControllerAction
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                    })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).GroupBy(x => x.Controller).ToList();

            return controlleractionlist;
        }


         
        [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
        public async Task<IEnumerable<NavigationMenu>> GetControllers()
        {
            var controlleractionlist = GetControllersFromAssembly();

            //Menus na Base de Dados 
            var treeRoot = await _context.NavigationMenus.AsNoTracking().ToListAsync();
            var list = new List<NavigationMenu>();

            int total = 0;
            foreach (var controller in controlleractionlist)
            {
                total++;
                var nodeController = new NavigationMenu
                {
                    ControllerName = controller.Key.Replace("Controller", string.Empty),
                    ActionName = "",
                    Id = Guid.NewGuid(),
                    Name = controller.Key.Replace("Controller", string.Empty),
                    ExternalUrl = ""
                };
                try
                {
                    //se existe na BD
                    nodeController.Id = treeRoot.Where(x => x.Name == nodeController.Name).First().Id;
                }
                catch (Exception)
                {
                    await _context.NavigationMenus.AddAsync(nodeController);
                    await _context.SaveChangesAsync();
                }

                foreach (var action in controller)
                {
                    total++;
                    var nodeAction = new NavigationMenu
                    {
                        ControllerName = action.Controller.Replace("Controller", string.Empty),
                        ActionName = action.Action,
                        Name = action.Action,
                        ParentMenuId = nodeController.Id, 
                        Id = Guid.NewGuid(),
                        ExternalUrl = "",
                        DisplayOrder = 1
                    };
                    try
                    {
                        //se existe na BD
                        nodeAction.Id = treeRoot.Where(x => x.Name == action.Action && x.ParentMenuId == nodeController.Id).First().Id;
                    }
                    catch (Exception)
                    {
                        await _context.NavigationMenus.AddAsync(nodeAction);
                        await _context.SaveChangesAsync();
                    }

                }
            }

            var navMenuDB = await _context.NavigationMenus.AsNoTracking().ToListAsync();
            if (navMenuDB.Count > total)
            {
                navMenuDB = await RemoveNotIncludedMenus(navMenuDB, controlleractionlist);
            }

            //UpdateMenus(list, treeRoot);


            return navMenuDB;
        }


        private async Task<List<NavigationMenu>> RemoveNotIncludedMenus(List<NavigationMenu> navMenuDB, List<IGrouping<string, ControllerAction>> controlleractionlist)
        {

            var navMenuDBList = ModelFactory.AsNavigationMenuNodeList(navMenuDB, null).ToList();
            foreach (var controller in navMenuDBList)
            {
                //se nao existir um controller
                string controllerName = controller.Node.Name + "Controller";
                if (!controlleractionlist.Exists(x => x.Key == controllerName))
                {
                    var controllerToDelete = await _context.NavigationMenus.FindAsync(controller.Node.Id);
                    var listToDelete = await _context.NavigationMenus.Where(x => x.ParentMenuId == controllerToDelete.Id).ToListAsync();
                    _context.NavigationMenus.RemoveRange(listToDelete);
                    await _context.SaveChangesAsync(User.Identity.Name);
                    _context.NavigationMenus.Remove(controllerToDelete);
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
                //Se existir, verifica cada Action
                else
                {
                    foreach (var action in controller.Children)
                    {
                        if (!controlleractionlist.First(k => k.Key == controllerName).Any(x => x.Action == action.Node.Name))
                        {
                            _context.NavigationMenus.Remove(new NavigationMenu { Id = action.Node.Id });
                            await _context.SaveChangesAsync(User.Identity.Name);
                        }
                    }
                }
            }
            //await _context.SaveChangesAsync(User.Identity.Name);
            return await _context.NavigationMenus.AsNoTracking().ToListAsync();
        }


        public class ControllerAction
        {
            public string Controller { get; set; }
            public string Action { get; set; }
            public string ReturnType { get; set; }
            public string Attributes { get; set; }
        }

    }
}