using _.Application.Responses.Identity;
using _.Domain.Contracts.Chat;
using _.Domain.Entities.Chat;
using _.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}