using _.Application.Features.Products.Commands.AddEdit;
using _.Application.Features.Products.Queries.GetAllPaged;
using _.Application.Requests.Catalog;
using _.Shared.Wrapper;
using System.Threading.Tasks;

namespace _.Client.Infrastructure.Managers.Catalog.Speed
{
    public interface ISpeedGovManager : IManager
    {
        Task<PaginatedResult<GetAllPagedSpeedGovernor>> GetSpeedGovernors(GetAllSpeedGovernorRequest request);
        Task<PaginatedResult<GetAllPagedLocation>> GetLocations(GetAllLocationRequest request);

        Task<IResult<string>> GetSpeedGovernorImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditSpeedGovCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}