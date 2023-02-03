using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Policy = DynamicPolicies.DynamicAdmin)]
    public class TecnologiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TecnologiasController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Tecnologias
        public async Task<IActionResult> Index()
        {
              return _context.Tecnologias != null ? 
                          View(await _context.Tecnologias.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tecnologias'  is null.");
        }

        // GET: Tecnologias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tecnologias == null)
            {
                return NotFound();
            }

            return View(tecnologias);
        }

        // GET: Tecnologias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tecnologias/Create        
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Sigla,Link,Linklogs,Linkdocs,Linkreports,AD,DB,Descricao,Maildev,ImageFile")] Tecnologias tecnologias)
        {
            //if (ModelState.IsValid)
           // {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(tecnologias.ImageFile.FileName);
                string extencion = Path.GetExtension(tecnologias.ImageFile.FileName);
                tecnologias.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extencion;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName); //tecnologias.
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await tecnologias.ImageFile.CopyToAsync(fileStream);
                }
                //Minuto 23:17 a verificar se dá create ps não deu tentar copiar tudo e adicionar o file name e refazer controlers e vews
                _context.Add(tecnologias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        //    }
          //  return View(tecnologias);
        }

        // GET: Tecnologias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias.FindAsync(id);
            if (tecnologias == null)
            {
                return NotFound();
            }
            return View(tecnologias);
        }

        // POST: Tecnologias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Sigla,Link,Linklogs,Linkdocs,Linkreports,AD,DB,Descricao,Maildev,ImageName")] Tecnologias tecnologias)
        {
            if (id != tecnologias.ID)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
           // {
                try
                {
                    _context.Update(tecnologias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TecnologiasExists(tecnologias.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
         //   }
           // return View(tecnologias);
        }

        // GET: Tecnologias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tecnologias == null)
            {
                return NotFound();
            }

            return View(tecnologias);
        }

        // POST: Tecnologias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tecnologias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tecnologias'  is null.");
            }
            var tecnologias = await _context.Tecnologias.FindAsync(id);
            if (tecnologias != null)
            {
                _context.Tecnologias.Remove(tecnologias);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TecnologiasExists(int id)
        {
          return (_context.Tecnologias?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
