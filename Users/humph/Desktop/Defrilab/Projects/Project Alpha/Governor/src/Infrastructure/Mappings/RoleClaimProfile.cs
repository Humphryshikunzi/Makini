using AutoMapper;
using _.Application.Requests.Identity;
using _.Application.Responses.Identity;
using _.Domain.Entities.Identity;

namespace _.Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, AppuserRoleClaims>()
                .ForMember(nameof(AppuserRoleClaims.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(AppuserRoleClaims.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, AppuserRoleClaims>()
                .ForMember(nameof(AppuserRoleClaims.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(AppuserRoleClaims.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}