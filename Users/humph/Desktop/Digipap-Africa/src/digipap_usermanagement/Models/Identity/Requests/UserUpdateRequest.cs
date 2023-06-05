
namespace Digipap.Models.Identity.Requests; 
  
public class UserUpdateRequest
{
    // main properties
    public  string? Id { get; set; }
     public string? Email { get; set; }  
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; } 
    public string? UserName { get; set; }
    public List<RoleWithUserRequest>? Roles { get; set; }

    // supplementary properties 
    public string?  UpdatedBy { get; set; } 
}
