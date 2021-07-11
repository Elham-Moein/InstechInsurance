using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InsuranceProject.Interfaces;
using InsuranceProject.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace InsuranceProject.Senders
{
    public class AuditMessageSender:IAuditMessageSender
    {
        private readonly IConfiguration _config;

        public AuditMessageSender(IConfiguration config)
        {
            _config = config;

        }
        
        

        public async Task SendMessage(ClaimAuditASB claimAuditAsb)
        {
            var queueClient = new QueueClient(_config.GetConnectionString("AzureServiceBus"), "ClaimAudit");
            
            var msgBody = JsonSerializer.Serialize(claimAuditAsb);
            
            var msg = new Message(Encoding.UTF8.GetBytes(msgBody));
            
            await queueClient.SendAsync(msg);

        }
        
        
    }
}