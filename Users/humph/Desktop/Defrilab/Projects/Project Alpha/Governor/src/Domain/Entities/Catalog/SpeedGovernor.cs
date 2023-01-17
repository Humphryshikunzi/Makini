using _.Domain.Contracts;
using _.Domain.Entities.Identity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace _.Domain.Entities.Catalog
{
    public class SpeedGovernor : AuditableEntity<int>
    {
        public string PlateNummber  { get; set; }
        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }
        public string PhoneNumber { get; set; }
        public int CartypeId { get; set; }
        public virtual Brand Cartype { get; set; }
        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}