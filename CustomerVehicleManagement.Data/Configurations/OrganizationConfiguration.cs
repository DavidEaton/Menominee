using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class OrganizationConfiguration : EntityConfiguration<Organization>
    {
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Organization", "dbo");
            builder.Ignore(p => p.TrackingState);

            // Value Object: Address
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.AddressLine)
                   .HasColumnName("AddressLine")
                   .HasMaxLength(255);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(255);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(15);
            builder.OwnsOne(p => p.Address)
                   .Property(p => p.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(255);
        }
    }
}
