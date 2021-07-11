using System.Threading.Tasks;
using InsuranceProject.Models;

namespace InsuranceProject.Interfaces
{
    public interface IAuditMessageSender
    {
        Task SendMessage(ClaimAuditASB claimAuditAsb);
    }
}