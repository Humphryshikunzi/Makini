using AutoMapper;
using _.Application.Features.Brands.Commands.AddEdit;
using _.Application.Features.Brands.Queries.GetAll;
using _.Application.Features.Brands.Queries.GetById;
using _.Domain.Entities.Catalog;

namespace _.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}