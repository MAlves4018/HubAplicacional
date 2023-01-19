using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Data;

namespace WebApp.Repositories
{
    public interface IAuditRepository
    {
       Task<IEnumerable<Audit>> GetAudits();
       Task<Audit> GetAudit(int id);
    }
}
