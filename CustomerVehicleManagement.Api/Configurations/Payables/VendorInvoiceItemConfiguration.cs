using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class VendorInvoiceItemConfiguration : EntityConfiguration<VendorInvoiceLineItem>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceLineItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoiceItem", "dbo");

            // Value Object: VendorInvoiceLineItem
            builder.OwnsOne(lineItem => lineItem.Item)
                   .Property(item => item.Description)
                   .HasColumnName("Description")
                   .HasMaxLength(255)
                   .IsRequired();
            builder.OwnsOne(lineItem => lineItem.Item)
                   .Property(item => item.PartNumber)
                   .HasColumnName("PartNumber")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.Type)
                   .HasDefaultValue(VendorInvoiceItemType.Purchase)
                   .IsRequired();
        }
    }
}
