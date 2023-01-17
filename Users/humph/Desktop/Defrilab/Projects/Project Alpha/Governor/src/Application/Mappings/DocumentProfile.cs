using AutoMapper;
using _.Application.Features.Documents.Commands.AddEdit;
using _.Application.Features.Documents.Queries.GetById;
using _.Domain.Entities.Misc;

namespace _.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}