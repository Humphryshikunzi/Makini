using Digipap.IRepositories;
using Digipap.Models.Identity.DbEntity;
using Digipap.Models.Settings;
using Digipap.Persistent;
using Digipap.Repositories;
using Digipap.Repositories.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;  
using System.Text;  

var builder = WebApplication.CreateBuilder(args);
var _configuration = builder.Configuration;
var services = builder.Services;

var appSettingsSection = _configuration.GetSection("AppSettings");
services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var appSettingsKey = Encoding.ASCII.GetBytes(appSettings.Key);

string connString = _configuration.GetConnectionString("ConnectionString") ?? throw new ArgumentNullException("No connection string provided");
services.AddDbContextPool<DigipapDbContext>(options => options.UseSqlServer(connString));
services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5; 
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = true;
})
 .AddEntityFrameworkStores<DigipapDbContext>()
 .AddDefaultTokenProviders();

services.AddAuthentication(au =>
        {
            au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            au.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.RequireHttpsMetadata = false;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(appSettingsKey),
                ValidIssuer = "Digipapafrica.com",
                ValidAudience = "DigipapMicroservices",
                ClockSkew = TimeSpan.Zero
            };
        });

services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>(); 
  
services.AddControllers().AddJsonOptions(
    options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    }
);
services.AddSignalR();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Digipap UserManagement",
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Email = "info@digipapafrica.com",
                Name = "infor  Digipap support",
                Url = new Uri("https://digipapafrica.com")
            },

            TermsOfService = new Uri("https://digipapafrica.com"),
            License = new OpenApiLicense()
            {
                Name = "MIT Licence",
                Url = new Uri("https://digipapafrica.com")
            },
            Description = "API for all the operations requiring Users information."

        });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
}); 

services.AddTransient<IDbInitializerRepository, DbInitializerRepository>(); 

var app = builder.Build(); 
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digipap");
    c.RoutePrefix = string.Empty;
});
 
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
}); 
app.Initialize();
app.Run();
