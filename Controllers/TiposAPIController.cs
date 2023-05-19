using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TiposAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipos>>> GetTipos()
        {
            if (_context.Tipos == null)
            {
                return NotFound();
            }
            return await _context.Tipos.ToListAsync();
        }

        // GET: api/TiposAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tipos>> GetTipos(int id)
        {
            if (_context.Tipos == null)
            {
                return NotFound();
            }
            var tipos = await _context.Tipos.FindAsync(id);

            if (tipos == null)
            {
                return NotFound();
            }

            return tipos;
        }

        // PUT: api/TiposAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipos(int id, Tipos tipos)
        {
            if (id != tipos.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposExists(id))
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

        // POST: api/TiposAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tipos>> PostTipos(Tipos tipos)
        {
            if (_context.Tipos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tipos'  is null.");
            }
            _context.Tipos.Add(tipos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipos", new { id = tipos.Id }, tipos);
        }

        // DELETE: api/TiposAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipos(int id)
        {
            if (_context.Tipos == null)
            {
                return NotFound();
            }
            var tipos = await _context.Tipos.FindAsync(id);
            if (tipos == null)
            {
                return NotFound();
            }

            _context.Tipos.Remove(tipos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TiposExists(int id)
        {
            return (_context.Tipos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
