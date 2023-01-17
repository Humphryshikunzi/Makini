using _.Domain.Contracts.Chat;
using _.Domain.Entities.Chat;
using _.Domain.Entities.Identity;

using AutoMapper;

namespace _.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<AppUser>>().ReverseMap();
        }
    }
}