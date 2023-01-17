using AutoMapper;
using _.Application.Responses.Identity;
using _.Domain.Entities.Identity;

namespace _.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, AppUserRole>().ReverseMap();
        }
    }
}