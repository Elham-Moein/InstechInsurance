using System.Threading.Tasks;
using InsuranceProject.Context;
using InsuranceProject.Interfaces;
using InsuranceProject.Models;

namespace InsuranceProject.Repository
{
    public class ClaimAuditRepository : IClaimAuditRepository
    {
        
        private readonly DbReadWriteContext _context;
        public ClaimAuditRepository(DbReadWriteContext context)
        {
            _context = context;
        }

        public async Task Add(ClaimAudit claimAudit)
        {
            await _context.ClaimAudits.AddAsync(claimAudit);
            await _context.SaveChangesAsync();
        }
    }
}