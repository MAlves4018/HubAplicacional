using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace SIGE.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IDynamicAuthorizationDataService _dataAccessService;

        public NavigationMenuViewComponent(IDynamicAuthorizationDataService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _dataAccessService.GetUserMenusAsync(HttpContext.User);

            return View(items);
        }
    }
}