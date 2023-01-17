using _.Application.Responses.Identity;
using _.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using _.Domain.Contracts.Chat;
using _.Domain.Entities.Chat;

namespace _.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
    }
}