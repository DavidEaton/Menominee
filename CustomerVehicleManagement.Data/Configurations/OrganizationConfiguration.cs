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
            builder.Ignore(organization => organization.TrackingState);

            builder.Property(organization => organization.Name)
                .HasMaxLength(255)
                .IsRequired();

            // Value Object: Address
            builder.OwnsOne(organization => organization.Address)
                   .Property(address => address.AddressLine)
                   .HasColumnName("AddressLine")
                   .HasMaxLength(255);
            builder.OwnsOne(organization => organization.Address)
                   .Property(address => address.City)
                   .HasColumnName("AddressCity")
                   .HasMaxLength(255);
            builder.OwnsOne(organization => organization.Address)
                   .Property(address => address.PostalCode)
                   .HasColumnName("AddressPostalCode")
                   .HasMaxLength(15);
            builder.OwnsOne(organization => organization.Address)
                   .Property(address => address.State)
                   .HasColumnName("AddressState")
                   .HasMaxLength(255);
        }
    }
}
