
namespace Digipap.Models.Identity.DbEntity;
 
public class RolePemissionDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Permission>? Permmissions { get; set; }
}