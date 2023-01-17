using _.Application.Specifications.Base;
using _.Domain.Entities.Catalog;

namespace _.Application.Specifications.Catalog
{
    public class ProductFilterSpecification : AppSpecification<SpeedGovernor>
    {
        public ProductFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Cartype);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.PlateNummber != null && (p.PhoneNumber.Contains(searchString) || p.Owner.FirstName.Contains(searchString)  || p.Owner.LastName.Contains(searchString)|| p.Cartype.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.PlateNummber != null;
            }
        }
    }
}