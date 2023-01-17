using System;
using System.Collections.Generic;
using _.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace _.Domain.Entities.Identity
{
    public class AppUserRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<AppuserRoleClaims> RoleClaims { get; set; }

        public AppUserRole() : base()
        {
            RoleClaims = new HashSet<AppuserRoleClaims>();
        }

        public AppUserRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<AppuserRoleClaims>();
            Description = roleDescription;
        }
    }
}