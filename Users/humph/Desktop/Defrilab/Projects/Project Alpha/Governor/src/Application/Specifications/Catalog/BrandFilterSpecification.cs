using _.Application.Specifications.Base;
using _.Domain.Entities.Catalog;

namespace _.Application.Specifications.Catalog
{
    public class BrandFilterSpecification : AppSpecification<Brand>
    {
        public BrandFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
