using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TecnologiasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TecnologiasAPIController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/TecnologiasAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tecnologias>>> GetTecnologias()
        {
            if (_context.Tecnologias == null)
            {
                return NotFound();
            }
            return await _context.Tecnologias.Include(t => t.Tipo).ToListAsync();
        }

        // GET: api/TecnologiasAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tecnologias>> GetTecnologias(int id)
        {
            if (_context.Tecnologias == null)
            {
                return NotFound();
            }
            var tecnologias = await _context.Tecnologias.FindAsync(id);

            if (tecnologias == null)
            {
                return NotFound();
            }

            return tecnologias;
        }

        // PUT: api/TecnologiasAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTecnologias(int id, Tecnologias tecnologias)
        {
            if (id != tecnologias.Id)
            {
                return BadRequest();
            }

            _context.Entry(tecnologias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TecnologiasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TecnologiasAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tecnologias>> PostTecnologias(Tecnologias tecnologias)
        {
            if (_context.Tecnologias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tecnologias'  is null.");
            }
            _context.Tecnologias.Add(tecnologias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTecnologias", new { id = tecnologias.Id }, tecnologias);
        }

        // DELETE: api/TecnologiasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTecnologias(int id)
        {
            if (_context.Tecnologias == null)
            {
                return NotFound();
            }
            var tecnologias = await _context.Tecnologias.FindAsync(id);
            if (tecnologias == null)
            {
                return NotFound();
            }

            _context.Tecnologias.Remove(tecnologias);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TecnologiasExists(int id)
        {
            return (_context.Tecnologias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
