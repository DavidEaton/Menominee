using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace CustomerVehicleManagement.Api.Users
{
    public class IdentityUserDbContext : IdentityUserContext<ApplicationUser>
    {
        public IdentityUserDbContext(DbContextOptions<IdentityUserDbContext> options)
            : base(options)
        { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
    }
}
