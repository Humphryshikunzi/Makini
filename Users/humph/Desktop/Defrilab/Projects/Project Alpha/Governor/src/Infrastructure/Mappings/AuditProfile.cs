using AutoMapper;
using _.Application.Responses.Audit;
using _.Domain.Entities.Audit;

namespace _.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}