using WebApp.Data;

namespace WebApp.Models
{
    public static class ModelFactory
    {
        public static List<NavigationMenu> AsNavigationMenuList(IEnumerable<NavigationMenuNode> nodes)
        {
            var itemList = new List<NavigationMenu>();

            foreach (var node in nodes)
            {
                var x = node.Node;
                itemList.Add(new NavigationMenu()
                {
                    Id =  x.Id,
                    Name = x.Name,
                    ControllerName=x.ControllerName,
                    ActionName=x.ActionName,
                    DisplayOrder=x.DisplayOrder,
                    ExternalUrl=x.ExternalUrl,
                    IsExternal=x.IsExternal,
                    ParentMenuId=x.ParentMenuId,
                    Visible=x.Visible,
                    NotAnActionOrController=x.NotAnActionOrController
                });
                itemList.AddRange(AsNavigationMenuList(node.Children));
            }

            return itemList;
        }

        public static List<NavigationMenuNode> AsNavigationMenuNodeList(IEnumerable<NavigationMenu> permissions, Guid? parent)
        {
            return permissions.Where(x => x.ParentMenuId == parent).OrderBy(x => x.DisplayOrder).ToList().Select(x =>
                    new NavigationMenuNode()
                    {
                        Node = new NavigationMenuModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ControllerName = x.ControllerName,
                            ActionName = x.ActionName,
                            DisplayOrder = x.DisplayOrder,
                            ExternalUrl = x.ExternalUrl,
                            IsExternal = x.IsExternal,
                            ParentMenuId = x.ParentMenuId,
                            Visible = x.Visible,
                            NotAnActionOrController = x.NotAnActionOrController
                        },
                        Children = AsNavigationMenuNodeList(permissions, x.Id)
                    })
                .ToList();
        }

        public static List<RolePermission> AsRolePermissionListSorted(IEnumerable<RolePermission> items, Guid? parent)
        {
            var itemList = new List<RolePermission>();

            var children = items.Where(x => x.ParentMenuId == parent);

            foreach (var node in children)
            {
                itemList.Add(node);
                itemList.AddRange(AsRolePermissionListSorted(items, node.Id));
            }

            return itemList;
        }

        public static NavigationMenuModel AsNavigationMenuModel(NavigationMenu x)
        {
            return new NavigationMenuModel()
            {
                Id = x.Id,
                Name = x.Name,
                ParentMenuId = x.ParentMenuId,
                ControllerName = x.ControllerName,
                ActionName = x.ActionName,
                IsExternal = x.IsExternal,
                ExternalUrl = x.ExternalUrl,
                DisplayOrder = x.DisplayOrder,
                Visible = x.Visible
            };
        }

    }
}