using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserMessages _userMessages;
        private readonly IDynamicAuthorizationDataService _dataAccessService;

        public HomeController(
            ApplicationDbContext context,
            ILogger<HomeController> logger,
            IUserMessages userMessages,
            IDynamicAuthorizationDataService dataAccessService)
        {
            _context = context;
            _logger = logger;
            _userMessages = userMessages;
            _dataAccessService = dataAccessService;
        }

        public async Task<IActionResult> Index()
        {
            if (User != null && User.Identity.Name != null && User.Identity.Name.Contains("\\"))
            {
                var nim = User.Identity.Name.Split("\\")[1];


                var user = await _context.Users.Where(u => u.UserName == nim).FirstOrDefaultAsync();
                var alertas = await _context.Alertas.Include(i => i.Users)
                        .Where(w => w.Ativo == true)
                        .Where(w => w.DataFim >= DateTime.Now && w.DataInicio <= DateTime.Now)
                        .Where(w => w.Users.Contains(user))
                        .ToListAsync();

                foreach (var alerta in alertas)
                {
                    _userMessages.AddUserMessage(alerta.CabecalhoAviso,
                        alerta.TextoAviso, IUserMessages.ErrorCode.INFO, 20000);
                }

            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult DashboardGeral()
        //{
        //	return View();
        //}

        //public IActionResult DashboardDiretiva()
        //{
        //	return View();
        //}

        //public IActionResult DashboardSetorial()
        //{
        //	return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}