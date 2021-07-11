using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InsuranceProject.Interfaces;
using InsuranceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Type = InsuranceProject.Models.Type;

namespace InsuranceProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsuranceController : ControllerBase
    {

        private readonly ILogger<InsuranceController> _logger;
        private readonly ICosmosDbRepository _cosmosDbRepository;
        private readonly IAuditMessageSender _auditMessageSender;
        public InsuranceController(ILogger<InsuranceController> logger, ICosmosDbRepository cosmosDbRepository,
            IAuditMessageSender auditMessageSender)
        {
            _logger = logger;
            _cosmosDbRepository = cosmosDbRepository;
            _auditMessageSender = auditMessageSender;

        }

        [HttpGet]
        public async Task<ActionResult> GetInsurance()
        {
            return Ok(await _cosmosDbRepository.GetItemsAsync());
        }
        
        [HttpPost]
        public async Task<ActionResult> AddInsurance(InsuranceClaim claim)
        {
            claim.Id = $"{claim.Name} {claim.Year}";
            var claimAudit = new ClaimAuditASB
            {
                ClaimId = claim.Id,
                TimeStamp = DateTime.Now,
                TypeOfRequest = "Post"
            };
            await _auditMessageSender.SendMessage(claimAudit);
            await _cosmosDbRepository.AddItemAsync(claim);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteInsurance(InsuranceClaim claim)
        {
            //Delete data from table
            await _cosmosDbRepository.DeleteItemAsync(claim.Id, claim.Name);
            var claimAudit = new ClaimAuditASB
            {
                ClaimId = claim.Id,
                TimeStamp = DateTime.Now,
                TypeOfRequest = "Delete"
            };
            await _auditMessageSender.SendMessage(claimAudit);
            return Ok();
        }
    }
}