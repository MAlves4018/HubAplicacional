using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models.ApplicationModels;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoTecnologiasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstadoTecnologiasAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EstadoTecnologiasAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoTecnologia>>> GetEstadosTecnologia()
        {
          if (_context.EstadosTecnologia == null)
          {
              return NotFound();
          }
            var estadosTecnologiasMaisRecentes =await _context.EstadosTecnologia
                .OrderByDescending(m => m.Timestamp)
                .ToArrayAsync();

            List<EstadoTecnologia> estadosMaisRecentesTecnologias = new List<EstadoTecnologia>();

            // Buscar tecnologias
            var todasTecnlogias = await _context.Tecnologias.Include(t => t.Tipo).ToListAsync();
           // var tipos= await _context.Tecnologias.Tipo.ToListAsync();

            if (todasTecnlogias != null)
            {
                foreach (var tecnologia in todasTecnlogias)
                {

                    var estadosTecnologiaMaisRecente = await _context.EstadosTecnologia
                    .Where(w => w.IdTecnologia == tecnologia.Id)
                    .OrderByDescending(m => m.Timestamp)
                    .FirstOrDefaultAsync();

                    if (estadosTecnologiaMaisRecente != null)
                    {
                        estadosMaisRecentesTecnologias.Add(estadosTecnologiaMaisRecente);
                    }

                }
            }
            return estadosMaisRecentesTecnologias.ToList();
        }

        // GET: api/EstadoTecnologiasAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoTecnologia>> GetEstadoTecnologia(int id)
        {
          if (_context.EstadosTecnologia == null)
          {
              return NotFound();
          }
            var estadoTecnologia = await _context.EstadosTecnologia.FindAsync(id);

            if (estadoTecnologia == null)
            {
                return NotFound();
            }
            //
            return estadoTecnologia;
        }

        // PUT: api/EstadoTecnologiasAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoTecnologia(int id, EstadoTecnologia estadoTecnologia)
        {
            if (id != estadoTecnologia.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoTecnologia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoTecnologiaExists(id))
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

        // POST: api/EstadoTecnologiasAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoTecnologia>> PostEstadoTecnologia(EstadoTecnologia estadoTecnologia)
        {
          if (_context.EstadosTecnologia == null)
          {
              return Problem("Entity set 'ApplicationDbContext.EstadosTecnologia'  is null.");
          }
            _context.EstadosTecnologia.Add(estadoTecnologia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoTecnologia", new { id = estadoTecnologia.Id }, estadoTecnologia);
        }

        // DELETE: api/EstadoTecnologiasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoTecnologia(int id)
        {
            if (_context.EstadosTecnologia == null)
            {
                return NotFound();
            }
            var estadoTecnologia = await _context.EstadosTecnologia.FindAsync(id);
            if (estadoTecnologia == null)
            {
                return NotFound();
            }

            _context.EstadosTecnologia.Remove(estadoTecnologia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoTecnologiaExists(int id)
        {
            return (_context.EstadosTecnologia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
