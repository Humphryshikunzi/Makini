using _.Application.Interfaces.Common;
using _.Application.Requests.Identity;
using _.Application.Responses.Identity;
using _.Shared.Wrapper;
using System.Threading.Tasks;

namespace _.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}