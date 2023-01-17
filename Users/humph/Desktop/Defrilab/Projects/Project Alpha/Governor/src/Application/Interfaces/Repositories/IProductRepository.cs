using System.Threading.Tasks;

namespace _.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<bool> IsCarTypeUsed(int brandId);
    }
}