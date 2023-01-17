using AutoMapper;
using _.Application.Responses.Identity;
using _.Domain.Entities.Identity;

namespace _.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, AppUser>().ReverseMap();
            CreateMap<ChatUserResponse, AppUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}