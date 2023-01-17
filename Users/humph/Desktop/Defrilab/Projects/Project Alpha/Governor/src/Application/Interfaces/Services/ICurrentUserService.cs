using _.Application.Interfaces.Common;

namespace _.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}