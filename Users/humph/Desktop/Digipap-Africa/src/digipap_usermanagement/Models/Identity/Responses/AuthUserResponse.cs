namespace Digipap.Models.Identity.Responses
{ 
    public class AuthUserResponse
    {
        public int Id { get; set; }
        public string? Token { get; set; } 
        public List<string>? Permissions {get; set;}
    }    
}
