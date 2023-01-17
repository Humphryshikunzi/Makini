using _.Application.Interfaces.Repositories;
using _.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<SpeedGovernor, int> _repository;

        public ProductRepository(IRepositoryAsync<SpeedGovernor, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsCarTypeUsed(int brandId)
        {
            return await _repository.Entities.AnyAsync(b => b.CartypeId == brandId);
        }
    }
}