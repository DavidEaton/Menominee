using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.Payables
{
    public class VendorConfiguration : EntityConfiguration<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Vendors", "dbo");

            // DO WE NEED NEXT LINE?
            // Yes, every class that inherits from Entity will need to ignore its TrackingState.
            builder.Ignore(vendor => vendor.TrackingState);

            builder.Property(vendor => vendor.VendorCode)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(vendor => vendor.Name)
                .HasMaxLength(255)
                .IsRequired();
            //builder.Property(vendor => vendor.IsActive)
            //    .HasDefaultValue(true);
        }
    }
}
