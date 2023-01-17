using _.Shared.Wrapper;
using System.Threading.Tasks;
using _.Application.Features.Dashboards.Queries.GetData;

namespace _.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}