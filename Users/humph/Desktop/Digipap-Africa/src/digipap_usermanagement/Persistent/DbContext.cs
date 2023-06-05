using Digipap.Models.DbEntities; 
using Digipap.Models.Identity.DbEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; 

namespace Digipap.Persistent
{
    public class DigipapDbContext : IdentityDbContext<User, Role, string>
    {
        public DigipapDbContext(DbContextOptions<DigipapDbContext> options) : base(options)
        {

        }
 
        public DbSet<Permission>? Permissions { get; set; }  
        public DbSet<UserReferal>? UserReferals { get; set; }
 

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                 entity.Property(t => t.Id).IsRequired();
            });

            modelbuilder.Entity<Permission>(entity =>
            {
                entity.ToTable(name:"Permissions", "Identity");
            });
            
            modelbuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");
            });

            modelbuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            modelbuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins", "Identity");
            });
            
            modelbuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles", "Identity");
            });

            modelbuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");

                entity.Property(t => t.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false); 

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(800)
                    .IsUnicode(false); 

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);
 
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelbuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }
}