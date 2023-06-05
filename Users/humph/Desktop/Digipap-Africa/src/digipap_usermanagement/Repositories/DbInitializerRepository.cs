using Digipap.IRepositories; 
using Digipap.Models.Identity.DbEntity;
using Digipap.Persistent;
using Microsoft.AspNetCore.Identity;  

namespace Digipap.Repositories;

public class DbInitializerRepository : IDbInitializerRepository
{   
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager; 
    private readonly  DigipapDbContext _dbContext;

    public DbInitializerRepository(UserManager<User> userManager, RoleManager<Role> roleManager, DigipapDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager; 
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        IdentityDbInitializer.SeedData(_userManager, _roleManager, _dbContext).Wait();
    }
}