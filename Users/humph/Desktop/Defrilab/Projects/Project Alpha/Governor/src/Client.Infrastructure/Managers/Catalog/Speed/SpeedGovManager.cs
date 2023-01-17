using _.Application.Features.Products.Commands.AddEdit;
using _.Application.Features.Products.Queries.GetAllPaged;
using _.Application.Requests.Catalog;
using _.Client.Infrastructure.Extensions;
using _.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace _.Client.Infrastructure.Managers.Catalog.Speed
     {
    public class SpeedGovManager : ISpeedGovManager
    {
        private readonly HttpClient _httpClient;

        public SpeedGovManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.SpeedGovernorEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.SpeedGovernorEndpoints.Export
                : Routes.SpeedGovernorEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

    public async Task<PaginatedResult<GetAllPagedLocation>> GetLocations(GetAllLocationRequest request)
    {
      var response = await _httpClient.GetAsync(Routes.SpeedGovernorEndpoints.GetAllLocationPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
      return await response.ToPaginatedResult<GetAllPagedLocation>();
    }

    public async Task<IResult<string>> GetSpeedGovernorImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.SpeedGovernorEndpoints.GetProductImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedSpeedGovernor>> GetSpeedGovernors(GetAllSpeedGovernorRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.SpeedGovernorEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedSpeedGovernor>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditSpeedGovCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SpeedGovernorEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}