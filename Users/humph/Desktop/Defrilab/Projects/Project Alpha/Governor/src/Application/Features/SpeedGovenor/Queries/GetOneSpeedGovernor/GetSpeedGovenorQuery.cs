using _.Application.Interfaces.Repositories;
using _.Application.Specifications.Catalog;
using _.Domain.Entities.Catalog;
using _.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _.Application.Features.SpeedGovenor.Queries.GetOneSpeedGovernor
{
    public class GetSpeedGovenorQuery : IRequest<Result<SpeedGovernor>>
    {
     
        public string SearchString { get; set; }
       

        public GetSpeedGovenorQuery (string searchString)
        {
           
            SearchString = searchString;
        
        }
    }

    internal class GetSpeedGovernorQueryHandler : IRequestHandler<GetSpeedGovenorQuery,Result<SpeedGovernor>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetSpeedGovernorQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SpeedGovernor>> Handle(GetSpeedGovenorQuery request, CancellationToken cancellationToken)
        {
       
            var productFilterSpec = new ProductFilterSpecification(request.SearchString);
            var data = await _unitOfWork.Repository<SpeedGovernor>().Entities
               .Where(sg => sg.PlateNummber == request.SearchString).FirstOrDefaultAsync();
            return await Result<SpeedGovernor>.SuccessAsync(data);
            }
        }
}
