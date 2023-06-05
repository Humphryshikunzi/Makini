
namespace Digipap.Models.Identity.Requests; 
  
public class UserPasswordResetRequest
{
    // main properties 
    public string? ConfirmPassword { get; set; }
    public string? Email { get; set; } 
    public string? Password { get; set; } 
    public string? UserName { get; set; }

    // supplementary properties 
    public DateTime UpdatedDate { get; set; }
}
