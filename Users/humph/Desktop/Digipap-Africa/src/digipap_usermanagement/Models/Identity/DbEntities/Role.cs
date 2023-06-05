using Microsoft.AspNetCore.Identity;

namespace Digipap.Models.Identity.DbEntity;

public class Role : IdentityRole<string>
{
    public  string?  Description { get; set; }
}