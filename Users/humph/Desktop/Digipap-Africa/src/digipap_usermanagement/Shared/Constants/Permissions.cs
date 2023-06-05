using Digipap.Models.Identity.DbEntity; 

namespace Digipap.Shared.Constants 
{
    public  class Permissions
    { 
        public static List<Permission> DefaultPermissions = new List<Permission>()
        {
            new Permission()
            {
                Name = "Permission.User.Add",
                Description = "Permission to add user"
            },
            new Permission()
            {
                Name = "Permission.User.View",
                Description = "Permission to view user(s)"
            }, 
            new Permission()
            {
                Name = "Permission.User.Edit",
                Description = "Permission to edit user(s)"
            },
            new Permission()
            {
                Name = "Permission.User.Delete",
                Description = "Permission to delete user(s)"
            }
        };
    }
}
