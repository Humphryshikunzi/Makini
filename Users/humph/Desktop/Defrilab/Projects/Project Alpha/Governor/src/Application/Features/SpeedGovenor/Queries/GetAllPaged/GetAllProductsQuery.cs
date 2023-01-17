using _.Application.Extensions;
using _.Application.Interfaces.Repositories;
using _.Application.Specifications.Catalog;
using _.Domain.Entities.Catalog;
using _.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace _.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllProductQuery : IRequest<PaginatedResult<GetAllPagedSpeedGovernor>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllProductQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductQuery, PaginatedResult<GetAllPagedSpeedGovernor>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedSpeedGovernor>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<SpeedGovernor, GetAllPagedSpeedGovernor>> expression = e => new GetAllPagedSpeedGovernor
            {
                Id = e.Id,
                PlateNumber = e.PlateNummber,
                PhoneNumber = e.PhoneNumber,
                CarType = e.Cartype.Name,
                OwnerName = e.Owner.FirstName,
                CarTypeId = e.CartypeId,
            };
            var productFilterSpec = new ProductFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<SpeedGovernor>().Entities
                   .Specify(productFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<SpeedGovernor>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}