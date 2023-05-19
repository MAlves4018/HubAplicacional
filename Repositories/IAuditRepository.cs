using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IAuditRepository
    {
        Task<IEnumerable<Audit>> GetAudits();
        Task<Audit> GetAudit(int id);
    }
}
