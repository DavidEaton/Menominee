using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Payables
{
    public class VendorConfiguration : EntityConfiguration<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Vendor", "dbo");

            builder.Ignore(vendor => vendor.TrackingState);

            builder.Property(vendor => vendor.VendorCode)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(vendor => vendor.Name)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
