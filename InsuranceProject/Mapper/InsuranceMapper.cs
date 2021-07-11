using InsuranceProject.Context;
using InsuranceProject.Models;
using AutoMapper;


namespace InsuranceProject.Mapper
{
    public class InsuranceMapper : Profile
    {
        public InsuranceMapper()
        {
            CreateMap<ClaimAudit, ClaimAuditASB>().ReverseMap();
        }
    }
}