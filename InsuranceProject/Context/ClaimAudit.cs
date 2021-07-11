using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceProject.Context
{
    
    [Table("ClaimAudit", Schema = "dbo")]
    public class ClaimAudit
    {
        [Key]
        public int Id { get; set; }
        public string ClaimId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TypeOfRequest { get; set; }
    }
}