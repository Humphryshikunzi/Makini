using Microsoft.AspNetCore.Identity;

namespace Digipap.Models.Identity.DbEntity;

public partial class User : IdentityUser<string>
{
    // Main properties

    public string? FirstName { get; set; }
    public string? LastName  { get; set; }
    public string? NationalId { get; set; } // allow passport 
    public string? ProfilePictureUrl { get; set; } 
    public string? ReferalCode { get; set; }
    
    public ICollection<User>? UserReferals { get; set; }


    // Supplementary properties 
    public string? CreatedBy { get; set; } 
    public DateTime? CreatedDate { get; set; } 
    public bool IsActive { get; set; }  = true;
    public bool IsConfirmed { get; set; }
    public bool  IsSuspended { get; set; }
    public string?  UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
  
