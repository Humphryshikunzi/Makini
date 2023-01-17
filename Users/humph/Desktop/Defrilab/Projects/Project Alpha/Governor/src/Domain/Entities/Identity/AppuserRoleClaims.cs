using System;
using _.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace _.Domain.Entities.Identity
{
    public class AppuserRoleClaims : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual AppUserRole Role { get; set; }

        public AppuserRoleClaims() : base()
        {
        }

        public AppuserRoleClaims(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}