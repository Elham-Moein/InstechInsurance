using System.Threading.Tasks;

namespace InsuranceProject.Interfaces
{
    public interface IAuditMessageConsumer
    {
        Task RegisterHandlers();
    }
}