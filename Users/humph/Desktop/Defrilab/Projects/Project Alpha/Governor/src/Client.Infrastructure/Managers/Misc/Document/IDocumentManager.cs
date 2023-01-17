using _.Application.Features.Documents.Commands.AddEdit;
using _.Application.Features.Documents.Queries.GetAll;
using _.Application.Requests.Documents;
using _.Shared.Wrapper;
using System.Threading.Tasks;
using _.Application.Features.Documents.Queries.GetById;

namespace _.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}