using _.Application.Specifications.Base;
using _.Domain.Entities.Catalog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _.Application.Specifications.Catalog
{
  public class LocationFilterSpecification:  AppSpecification<Location>
  {
    public LocationFilterSpecification(string searchString)
    {
      if (!string.IsNullOrEmpty(searchString))
      {
        Criteria = p => p.SpeedGovId.ToString().Contains(searchString) || p.Time.Contains(searchString);
      }
      else
      {
        Criteria = p => true;
      }
    }
  }
}
