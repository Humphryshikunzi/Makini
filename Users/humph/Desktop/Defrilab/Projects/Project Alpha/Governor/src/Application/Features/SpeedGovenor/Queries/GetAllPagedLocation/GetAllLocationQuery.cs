using _.Application.Extensions;
using _.Application.Interfaces.Repositories;
using _.Application.Specifications.Catalog;
using _.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace _.Application.Features.Products.Queries.GetAllPaged
{
using _.Domain.Entities.Catalog;
  public class GetAllLocationQuery : IRequest<PaginatedResult<GetAllPagedLocation>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

    public GetAllLocationQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

  internal class GetAllLocationQueryHandler : IRequestHandler<GetAllLocationQuery, PaginatedResult<GetAllPagedLocation>>
  {
    private readonly IUnitOfWork<int> _unitOfWork;

    public GetAllLocationQueryHandler(IUnitOfWork<int> unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResult<GetAllPagedLocation>> Handle(GetAllLocationQuery request, CancellationToken cancellationToken)
    {

      Expression<Func<Location, GetAllPagedLocation>> expression = e => new GetAllPagedLocation
      {
        Date = e.Date,
        Latitude = e.Latitude,
        Time = e.Time,
        Speed = e.Speed,
        Long = e.Long,
        GpsCourse = e.GpsCourse,
        SpeedSignalStatus = e.SpeedSignalStatus,
        PlateNumber = e.SpeedGovernor.PlateNummber
      };
      var locationFilter = new LocationFilterSpecification(request.SearchString);
      if (request.OrderBy?.Any() != true)
      {
        var data = await _unitOfWork.Repository<Location>().Entities
           .OrderByDescending(x =>x.Id)
           .Specify(locationFilter)
           .Select(expression)           
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return data;
      }
      else
      {
        var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
        var data = await _unitOfWork.Repository<Location>().Entities
           .Specify(locationFilter)
           .OrderBy(ordering)
           .OrderByDescending( x => x.Id)// require system.linq.dynamic.core
           .Select(expression)
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return data;

      }
    }
  }
}