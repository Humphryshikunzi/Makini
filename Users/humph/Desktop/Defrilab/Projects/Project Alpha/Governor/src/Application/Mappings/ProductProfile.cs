using AutoMapper;
using _.Application.Features.Products.Commands.AddEdit;
using _.Domain.Entities.Catalog;

namespace _.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditSpeedGovCommand, SpeedGovernor>().ReverseMap();
        }
    }
}