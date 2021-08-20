using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Menominee.Idp.Data.Contexts
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AspNetTenant> AspNetTenants { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<AspNetTenant>()
                .HasAlternateKey(tenant => tenant.Name)
                .HasName("AlternateKey_Name");
        }
    }
}
