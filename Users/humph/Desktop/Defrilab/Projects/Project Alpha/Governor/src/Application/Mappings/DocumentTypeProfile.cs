using AutoMapper;
using _.Application.Features.DocumentTypes.Commands.AddEdit;
using _.Application.Features.DocumentTypes.Queries.GetAll;
using _.Application.Features.DocumentTypes.Queries.GetById;
using _.Domain.Entities.Misc;

namespace _.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}