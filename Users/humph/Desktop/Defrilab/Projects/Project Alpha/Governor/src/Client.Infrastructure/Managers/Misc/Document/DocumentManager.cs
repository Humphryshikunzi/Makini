﻿using _.Application.Features.Documents.Commands.AddEdit;
using _.Application.Features.Documents.Queries.GetAll;
using _.Application.Requests.Documents;
using _.Client.Infrastructure.Extensions;
using _.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using _.Application.Features.Documents.Queries.GetById;

namespace _.Client.Infrastructure.Managers.Misc.Document
{
    public class DocumentManager : IDocumentManager
    {
        private readonly HttpClient _httpClient;

        public DocumentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DocumentsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllDocumentsResponse>();
        }

        public async Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request)
        {
            var response = await _httpClient.GetAsync(Routes.DocumentsEndpoints.GetById(request.Id));
            return await response.ToResult<GetDocumentByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DocumentsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}