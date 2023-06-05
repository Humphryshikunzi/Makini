using Digipap.Models.Identity.DbEntity;
using Digipap.Shared.Constants;
using Microsoft.AspNetCore.Identity;  
using System.Security.Claims; 

namespace  Digipap.Persistent
{
    public static class IdentityDbInitializer
    { 
        public static async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager, DigipapDbContext dbContext)
        {  
            await SeedPermissions(userManager,dbContext);
            await SeedRoles(roleManager);
            await SeedUsers(userManager,roleManager);
        }

        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            foreach (var user in TestUser.TestUsers)
            {
                await CreateUserAsync(userManager, roleManager, user,  user.PasswordHash, Permissions.DefaultPermissions);  
            }
        }

        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
              foreach (var role in  Roles.DefaultRoles)
              {
                await CreateRoleAsync(roleManager, role);
              }
        }
      
        public static async Task SeedPermissions(UserManager<User> userManager, DigipapDbContext dbContext)
        {
             if (!userManager.Users.Any())
             {
                foreach (var  permission in Permissions.DefaultPermissions)
                {
                    await dbContext.Permissions!.AddAsync(permission);
                    await dbContext.SaveChangesAsync();
                }
             }
        }

        private static async Task CreateRoleAsync(RoleManager<Role> roleManager, Role roleInput)
        {
            if (!await roleManager.RoleExistsAsync(roleInput.Name))
            {
                var role = new Role
                {
                    Name = roleInput.Name, 
                    Description = roleInput.Description
                };
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task CreateUserAsync(UserManager<User> userManager, RoleManager<Role> roleManager, User user, string password, List<Permission> permissions)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    foreach (var  role in Roles.DefaultRoles)
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                        await AddPermissionClaim(roleManager, role.Name!, permissions);
                    }
                }
            }
        }

        public static async Task AddPermissionClaim(RoleManager<Role> roleManager, string role, List<Permission> permissions)
        {   
            var posRole = await roleManager.FindByNameAsync(role);
            var allClaims = await roleManager.GetClaimsAsync(posRole);

            foreach (Permission  permission in permissions)
            {
                if(!allClaims.Any(a => a.Type == "Permission"  && a.Value == permission.Name))
                {
                    await roleManager.AddClaimAsync(posRole, new Claim("Permission", permission.Name!, "", issuer:"Digipapafrica.com"));
                }
            }            
        } 
    }
}

