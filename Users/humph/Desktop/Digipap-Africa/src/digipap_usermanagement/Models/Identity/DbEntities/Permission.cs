using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations; 

namespace Digipap.Models.Identity.DbEntity;

public partial class Permission 
{
    [Key]
    public int Id {get;set;}  
    public string? Name { get; set; }
    public string?  Description { get; set; } 
}
 
internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}