using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceProject.Interfaces;
using InsuranceProject.Models;
using Microsoft.Azure.Cosmos;

namespace InsuranceProject.Repository
{
    public class CosmosDbRepository : ICosmosDbRepository
    {
        private readonly Container _container;
        public CosmosDbRepository(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }
        
        public async Task AddItemAsync(InsuranceClaim claim)
        {
            await _container.CreateItemAsync<InsuranceClaim>(claim, new PartitionKey(claim.Name));
        }

        public async Task DeleteItemAsync(string id, string name)
        {
            await _container.DeleteItemAsync<InsuranceClaim>(id, new PartitionKey(name));
        }

        public async Task<List<InsuranceClaim>> GetItemsAsync()
        {
            try
            {
                var sqlQueryText = "SELECT * FROM c";
                var query = _container.GetItemQueryIterator<InsuranceClaim>(new QueryDefinition(sqlQueryText));
                List<InsuranceClaim> insuranceClaims = new List<InsuranceClaim>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    insuranceClaims.AddRange(response.ToList());
                }

                return insuranceClaims;
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            { 
                return null;
            }

        }
        
        
    }
}