using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceProject.Models;

namespace InsuranceProject.Interfaces
{
    public interface ICosmosDbRepository
    {
        Task AddItemAsync(InsuranceClaim item);
        Task DeleteItemAsync(string id, string name);
        Task<List<InsuranceClaim>> GetItemsAsync();
    }
}