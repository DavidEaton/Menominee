using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using SharedKernel.Enums;
using System;

namespace Menominee.Idp.Data.Contexts
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tenant> AspNetTenants { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Tenant>()
                .HasAlternateKey(tenant => tenant.Name)
                .HasName("AlternateKey_Name");

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", "dbo");

            modelBuilder.Entity<ApplicationUser>()
                .Property(user => user.ShopRole)
                   .HasMaxLength(50)
                   .HasConversion(
                        stringType => stringType.ToString(),
                        stringType => (ShopRole)Enum.Parse(typeof(ShopRole), stringType));
        }
    }
}
