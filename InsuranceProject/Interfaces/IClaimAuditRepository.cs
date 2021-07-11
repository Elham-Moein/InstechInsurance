using System.Threading.Tasks;
using InsuranceProject.Context;

namespace InsuranceProject.Interfaces
{
    public interface IClaimAuditRepository
    {
        Task Add(ClaimAudit claimAudit);
    }
}