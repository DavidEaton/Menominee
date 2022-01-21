using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class VendorInvoiceItemConfiguration : EntityConfiguration<VendorInvoiceItem>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceItem> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoiceItem", "dbo");

            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.PartNumber)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.MfrId)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(item => item.Description)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.Type)
                   .HasDefaultValue(VendorInvoiceItemType.Purchase)
                   .IsRequired();
        }
    }
}
