
namespace Digipap.Models.Identity.DbEntity;
 
public class UserRegisterRequest
{
    // main properties
    public string? ConfirmPassword { get; set; }
    public string? Email { get; set; }  
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }   
    public string? UserName { get; set; }
    public List<RoleWithUserRequest>? Roles { get; set; }

    // supplementary properties
    public string? CreatedBy { get; set; }
}
 