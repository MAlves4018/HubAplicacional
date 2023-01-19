using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class AuditsController : Controller
    {
        private readonly IAuditRepository _repository;

        public AuditsController(IAuditRepository repository)
        {
            _repository = repository;
        }

        // GET: Audits
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAudits());
        }

        // GET: Audits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audit = await _repository.GetAudit((int)id);
            if (audit == null)
            {
                return NotFound();
            }

            return View(audit);
        }
         
    }
}
