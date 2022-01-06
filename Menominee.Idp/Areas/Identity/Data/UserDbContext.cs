using Menominee.Common.Entities;
using Menominee.Common.Enums;
using Menominee.Idp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            modelBuilder.Entity<Tenant>()
                .HasAlternateKey(tenant => tenant.Name)
                .HasName("AlternateKey_Name");

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", "dbo")
                .HasIndex(user => user.UserName)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", "dbo")
                .Property(user => user.ShopRole)
                .HasConversion(
                    stringType => stringType.ToString(),
                    stringType => (ShopRole)Enum.Parse(typeof(ShopRole), stringType));

            modelBuilder.Entity<UserClaim>()
                .ToTable("AspNetUserClaims", "dbo")
                .Property(claim => claim.ApplicationUserId)
                .HasColumnName("UserId")
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                                 .Where(entry => entry.State == EntityState.Modified)
                                 .OfType<IConcurrencyAware>();

            foreach (var entry in updatedConcurrencyAwareEntries)
            {
                entry.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
