using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;

namespace WebApp.Controllers
{
    public class DeletedtecnologiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeletedtecnologiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Deletedtecnologies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Deletedtecnologies.Include(d => d.Tipo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Deletedtecnologies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deletedtecnologies == null)
            {
                return NotFound();
            }

            var deletedtecnologies = await _context.Deletedtecnologies
                .Include(d => d.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deletedtecnologies == null)
            {
                return NotFound();
            }

            return View(deletedtecnologies);
        }

        // GET: Deletedtecnologies/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name");
            return View();
        }

        // POST: Deletedtecnologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Sigla,Link,Linkdocs,Linklogs,Linkreports,Descricao,Maildev,TypeId,ImageName,Apagado")] Deletedtecnologies deletedtecnologies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deletedtecnologies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name", deletedtecnologies.TypeId);
            return View(deletedtecnologies);
        }

        // GET: Deletedtecnologies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deletedtecnologies == null)
            {
                return NotFound();
            }

            var deletedtecnologies = await _context.Deletedtecnologies.FindAsync(id);
            if (deletedtecnologies == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name", deletedtecnologies.TypeId);
            return View(deletedtecnologies);
        }

        // POST: Deletedtecnologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sigla,Link,Linkdocs,Linklogs,Linkreports,Descricao,Maildev,TypeId,ImageName,Apagado")] Deletedtecnologies deletedtecnologies)
        {
            if (id != deletedtecnologies.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deletedtecnologies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeletedtecnologiesExists(deletedtecnologies.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name", deletedtecnologies.TypeId);
            return View(deletedtecnologies);
        }

        // GET: Deletedtecnologies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deletedtecnologies == null)
            {
                return NotFound();
            }

            var deletedtecnologies = await _context.Deletedtecnologies
                .Include(d => d.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deletedtecnologies == null)
            {
                return NotFound();
            }

            return View(deletedtecnologies);
        }

        // POST: Deletedtecnologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deletedtecnologies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Deletedtecnologies'  is null.");
            }
            var deletedtecnologies = await _context.Deletedtecnologies.FindAsync(id);
            if (deletedtecnologies != null)
            {
                _context.Deletedtecnologies.Remove(deletedtecnologies);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeletedtecnologiesExists(int id)
        {
          return (_context.Deletedtecnologies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
