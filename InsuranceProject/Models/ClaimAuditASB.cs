using System;

namespace InsuranceProject.Models
{
    public class ClaimAuditASB
    {
        public string ClaimId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TypeOfRequest { get; set; }
    }
}