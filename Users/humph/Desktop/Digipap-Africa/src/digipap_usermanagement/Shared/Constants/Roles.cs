using Digipap.Models.Identity.DbEntity;

namespace Digipap.Shared.Constants 
{
    public class Roles
    { 
        public static List<Role> DefaultRoles = new List<Role>()
        {
            new Role()
            {
                Name = "SuperAdmin", 
                Description = "Has All Permissions"
            },
            new Role()
            {
                Name = "User", 
                Description = "Has least permissions"
            }
        };
    }
}
