using Digipap.Models.Identity.DbEntity; 
using Digipap.Models.Settings;
using Digipap.Persistent;
using Digipap.Models.Identity.Requests;  
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; 
using System.Security.Claims;

namespace  Digipap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly DigipapDbContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(SignInManager<User> signInManager, RoleManager<Role> roleManager, IAuthorizationService authorizationService,
         IOptions<AppSettings> appSettings, DigipapDbContext dbContext)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }
 
        [HttpPost]
        [Route("GetUserData")] 
        public async Task<IActionResult> GetUserData([FromBody] AuthUserRequest authDto)
        {
            User user = await _signInManager.UserManager.FindByNameAsync(authDto.UserName);
            if (user == null) return NotFound($"User by name {authDto.UserName} could not be found"); 
            return Ok(user);
        }
 
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userEntityDto)
        {
            var userWithSameUserName = await _signInManager.UserManager.FindByNameAsync(userEntityDto.UserName);
            if (userWithSameUserName != null)
            {
                return Conflict(string.Format("Username {0} is already taken.", userEntityDto.UserName));
            }

            if (!string.IsNullOrWhiteSpace(userEntityDto.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == userEntityDto.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    return Conflict(string.Format("Phonenumber {0} is already registered.", userEntityDto.PhoneNumber));
                }
            }

            if (!string.IsNullOrWhiteSpace(userEntityDto.NationalId))
            {
                var userWithSameDocumentNumber = await _signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.NationalId == userEntityDto.NationalId);
                if (userWithSameDocumentNumber != null)
                {
                    return Conflict(string.Format("User with NationalId {0} is already registered", userEntityDto.NationalId));
                }
            }

            if (!string.IsNullOrWhiteSpace(userEntityDto.Email))
            {
                var userWithSameEmail = await _signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userEntityDto.Email.ToLower());
                if (userWithSameEmail != null)
                {
                    return Conflict(string.Format("Email {0} is already registered.", userEntityDto.Email));
                }
            }
             if (userEntityDto.Password != userEntityDto.ConfirmPassword)
            {
                return Conflict(string.Format("Password {0} must be same as {1}.", userEntityDto.Password, userEntityDto.ConfirmPassword));
            }

            var userToSave = new User()
            {
                PasswordHash = userEntityDto.Password,
                UserName = userEntityDto.UserName,
                Email = userEntityDto.Email,
                CreatedBy = userEntityDto.CreatedBy,
                NationalId = userEntityDto.NationalId,
                FirstName = userEntityDto.FirstName,
                LastName = userEntityDto.LastName, 
                CreatedDate = DateTime.Now
            };

            var result = await _signInManager.UserManager.CreateAsync(userToSave, userToSave.PasswordHash);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(userToSave.Email);
                var dbRoles =  _roleManager.Roles.ToList();                 
                var dbRoleNames = new List<string>();
                dbRoles.ForEach(role=> dbRoleNames.Add(role.Name));
                
                List<string> errors = new List<string>();
                
                foreach (var role in userEntityDto.Roles!)
                {                    
                    if(dbRoleNames.Contains(role.Name))
                    {
                        await _signInManager.UserManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        errors.Add($"Role with name {role.Name} not found");
                    }
                }

                if(errors.Count>0)
                {
                    return BadRequest($"User has been created, even though these errors exist. Update the user to fix them. \n {string.Join('\n', errors)}");
                }
 
                return Created("",user);
            }

            return BadRequest();
        }
        
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _signInManager.UserManager.Users.Include(t=>t.UserReferals)!.ToListAsync();
        }

        // GET: api/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(string userId)
        {
            var user = await _dbContext.Users.Include(t=>t.UserReferals)!.Where(u=>u.Id==userId).FirstOrDefaultAsync();
            if (user == null) return NotFound();
            return user;
        }

        [HttpPut("updateUser")] 
        public async Task<IActionResult> UpdateUser(UserUpdateRequest userEntityDto)
        {
            var userToUpdate = await _signInManager.UserManager.FindByIdAsync(userEntityDto.Id);
            if (userToUpdate == null) return NotFound();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(userToUpdate);

            userToUpdate.FirstName = userEntityDto.FirstName;
            userToUpdate.LastName = userEntityDto.LastName;
            userToUpdate.Email = userEntityDto.Email;
            userToUpdate.PhoneNumber = userEntityDto.PhoneNumber; 
            userToUpdate.UserName = userEntityDto.UserName;  
            userToUpdate.UpdatedDate = DateTime.Now;
            userToUpdate.UpdatedBy = userEntityDto.UpdatedBy;

            _dbContext.Entry(userToUpdate).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();  

                // first remove user from current roles
                foreach (var role in userRoles)
                {
                    await _signInManager.UserManager.RemoveFromRoleAsync(userToUpdate, role);
                }

                // now add user to new roles
                foreach (var role in userEntityDto.Roles!)
                {
                    await _signInManager.UserManager.AddToRoleAsync(userToUpdate, role.Name);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Users.Any(u => u.Id == userToUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(userToUpdate);
        }

        [HttpDelete("delete")] 
        public IActionResult Delete(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                _dbContext.Remove(user);
                _dbContext.SaveChanges();
                return Ok(user);
            }

            return BadRequest($"Error, could not delete the user with id {id}. The user is not found in the database.");
        }
       
        #region  Roles

        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole([FromBody] RolePemissionDto roleDto)
        {
            var roleExists = await _roleManager.FindByNameAsync(roleDto.Name);
            if (roleExists != null) return Conflict("This role already exists in the db");

            Role role = new Role()
            {
                Name = roleDto.Name,
                Description = roleDto.Description
            };
            await _roleManager.CreateAsync(role);
            await IdentityDbInitializer.AddPermissionClaim(_roleManager, role.Name!, permissions: roleDto.Permmissions!);

            return Created("", role);
        }

        [HttpGet]
        [Route("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleManager.Roles.ToListAsync());
        }

        #endregion

        #region  Permissions

        [HttpPost]
        [Route("createPermissions")] 
        public async Task<IActionResult> CreatePermissions([FromBody] List<Permission> permissions)
        {
            var currentPermissions = await _dbContext.Permissions!.ToListAsync();
            List<string> currentPermissionsNames = new List<string>();
            currentPermissions.ForEach(permission => currentPermissionsNames.Add(permission.Name!));

            foreach (var permission in permissions)
            {
                if (currentPermissionsNames.Contains(permission.Name!)) continue;
                await _dbContext.Permissions!.AddAsync(permission);
            }

            await _dbContext.SaveChangesAsync();
            return Created("", permissions);
        }

        [HttpGet]
        [Route("getPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            string emailaddress = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")!.Value;
            var user = await _signInManager.UserManager.FindByEmailAsync(emailaddress);
            var roles = _roleManager.Roles.ToList();
            List<Claim> allClaims = new List<Claim>();

            foreach (var role in roles)
            {
                if (await _signInManager.UserManager.IsInRoleAsync(user, role.Name))
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    allClaims.AddRange(roleClaims.ToList());
                }
            }

            //if (!(_authorizationService.AuthorizeAsync(principal, "Permission.HR(Human Resource).Add")).Result.Succeeded) return Unauthorized();
            return Ok(await _dbContext.Permissions!.ToListAsync());
        }

        [HttpGet]
        [Route("getClaims")]
        public async Task<IActionResult> GetClaims()
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            string emailaddress = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")!.Value;
            var user = await _signInManager.UserManager.FindByEmailAsync(emailaddress);
            var roles = _roleManager.Roles.ToList();
            List<Claim> allClaims = new List<Claim>();

            foreach (var role in roles)
            {
                if (await _signInManager.UserManager.IsInRoleAsync(user, role.Name))
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    allClaims.AddRange(roleClaims.ToList());
                }
            }
 
            return Ok(allClaims);
        }

        #endregion
    }
}
