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
        //private const string V = "Portapessoal.png";
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
            var applicationDbContext = _context.Tecnologias.Include(t => t.Tipo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tecnologias
        public async Task<IActionResult> MonitorizacaoPage()
        {
            return View();
        }


        // GET: Tecnologias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias
                .Include(t => t.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnologias == null)
            {
                return NotFound();
            }

            return View(tecnologias);
        }

        // GET: Tecnologias/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name");
            return View();
        }

        // POST: Tecnologias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Name,Sigla,Link,Linkdocs,Linklogs,Linkreports,Descricao,Maildev,TypeId,ImageFile")] Tecnologias tecnologias)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (tecnologias.ImageFile != null)
            {
               // string wwwRootPath = _hostEnvironment.WebRootPath;
                var fileName = Path.GetFileNameWithoutExtension(tecnologias.ImageFile.FileName);
                string extencion = Path.GetExtension(tecnologias.ImageFile.FileName);
                tecnologias.ImageName = fileName = fileName + extencion;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName); //tecnologias.

                //Console.WriteLine(tecnologias.ImageFile.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await tecnologias.ImageFile.CopyToAsync(fileStream);
                }

                //var files = Directory.GetFiles("C:\\Users\\255667182\\source\\repos\\HubAplicacional\\wwwroot\\Image\\");
                //foreach (var item in files)
                //{
                //    if (item== tecnologias.ImageFile.FileName)
                //    {
                //        var fileName = Path.GetFileNameWithoutExtension(tecnologias.ImageFile.FileName);
                //        string extencion = Path.GetExtension(tecnologias.ImageFile.FileName);
                //        tecnologias.ImageName = fileName = fileName + extencion;
                //        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                //    }
                //    else
                //    {

                //        var fileName = Path.GetFileNameWithoutExtension(tecnologias.ImageFile.FileName);
                //        string extencion = Path.GetExtension(tecnologias.ImageFile.FileName);
                //        tecnologias.ImageName = fileName = fileName + extencion;
                //        string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                //        using (var fileStream = new FileStream(path, FileMode.Create))
                //        {
                //            await tecnologias.ImageFile.CopyToAsync(fileStream);
                //        }
                //    }
                //}
            }
            else
            {
                tecnologias.ImageName = "Defaultimage.jpg";
            }
            _context.Add(tecnologias);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            ViewData["TypeId"] = new SelectList(_context.Tipos, "Id", "Name", tecnologias.TypeId);
            return View(tecnologias);
        }

        // POST: Tecnologias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sigla,Link,Linkdocs,Linklogs,Linkreports,Descricao,Maildev,TypeId,ImageName,ImageFile")] Tecnologias tecnologias)//ImageName,
        {
            if (id != tecnologias.Id)
            {
                return NotFound();
            }
            if (tecnologias.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var fileName = Path.GetFileNameWithoutExtension(tecnologias.ImageFile.FileName);
                string extencion = Path.GetExtension(tecnologias.ImageFile.FileName);
                tecnologias.ImageName = fileName = fileName + extencion;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName); //tecnologias.

                //Console.WriteLine(tecnologias.ImageFile.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await tecnologias.ImageFile.CopyToAsync(fileStream);
                }

            }
            else
            {
                var antigoregistodatec = await _context.Tecnologias
                .Where(m => m.Id == tecnologias.Id)
                .OrderByDescending(m => m.Id)
                .ToArrayAsync();
                Console.WriteLine(antigoregistodatec);
                Console.WriteLine(antigoregistodatec[0].ImageName);

                Console.WriteLine(antigoregistodatec[0]);
                antigoregistodatec[0].Id = tecnologias.Id;
                antigoregistodatec[0].Name = tecnologias.Name;
                antigoregistodatec[0].Sigla = tecnologias.Sigla;
                antigoregistodatec[0].Link = tecnologias.Link;
                antigoregistodatec[0].Linkdocs = tecnologias.Linkdocs;
                antigoregistodatec[0].Linklogs = tecnologias.Name;
                antigoregistodatec[0].Linkreports = tecnologias.Linkreports;
                antigoregistodatec[0].Descricao = tecnologias.Descricao;
                antigoregistodatec[0].Maildev = tecnologias.Maildev;
                antigoregistodatec[0].TypeId = tecnologias.TypeId;
                tecnologias = antigoregistodatec[0];
            }
            try
            {
                _context.Update(tecnologias);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TecnologiasExists(tecnologias.Id))
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

        // GET: Tecnologias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias
                .Include(t => t.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnologias == null)
            {
                return NotFound();
            }
            //tecnologias.Apagado = true;
            return View(tecnologias);
        }
        public async Task<IActionResult> DeleteRepor(int? id)
        {
            if (id == null || _context.Tecnologias == null)
            {
                return NotFound();
            }

            var tecnologias = await _context.Tecnologias
                .Include(t => t.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
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
                if (tecnologias.Apagado == true)
                {
                    tecnologias.Apagado = false;
                }
                else
                {
                    tecnologias.Apagado = true;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TecnologiasExists(int id)
        {
            return (_context.Tecnologias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
