using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using InsuranceProject.Context;
using InsuranceProject.Interfaces;
using InsuranceProject.Models;
using InsuranceProject.Repository;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceProject.Consumers
{
    public class AuditMessageConsumer:IAuditMessageConsumer
    {
        private readonly IConfiguration _config;
        private readonly QueueClient _client;
        private readonly IClaimAuditRepository _claimAuditRepository;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        public AuditMessageConsumer(IConfiguration config, IMapper mapper, IServiceProvider serviceProvider)
        {
            _config = config;
            var connectionString = _config.GetConnectionString("AzureServiceBus");
            _client = new QueueClient(connectionString, "ClaimAudit");
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task RegisterHandlers()
        {
            var msgHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // hpw many messages we want to process at a time
                MaxConcurrentCalls = 1,
                // needs to wait until the message is processed not only read
                AutoComplete = false
            };

            _client.RegisterMessageHandler(HandleMessage, msgHandlerOptions);
        }

        private async Task HandleMessage(Message msg, CancellationToken token)
        {
            var jsonString = Encoding.UTF8.GetString(msg.Body);
            var claimAuditAsb = JsonSerializer.Deserialize<ClaimAuditASB>(jsonString);
            var claimAudit = _mapper.Map<ClaimAudit>(claimAuditAsb);
            await _client.CompleteAsync(msg.SystemProperties.LockToken);
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IClaimAuditRepository>();

                await context.Add(claimAudit);
            }
            
        }
        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            return Task.CompletedTask;
        }
    }
}