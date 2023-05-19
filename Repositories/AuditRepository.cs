using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<Audit> GetAudit(int id)
        {
            var audit = await _context.AuditLogs.FirstOrDefaultAsync(m => m.Id == id);
            return audit;
        }


        public async Task<IEnumerable<Audit>> GetAudits()
        {
            return await _context.AuditLogs.ToListAsync();
        }
    }
}
