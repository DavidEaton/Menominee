using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Payables
{
    public class VendorInvoiceLineItemConfiguration : EntityConfiguration<VendorInvoiceLineItem>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceLineItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoiceLineItem", "dbo");

            builder.Property(lineItem => lineItem.Type)
                .IsRequired();

            builder.Property(lineItem => lineItem.Quantity)
                .IsRequired();

            // Value Object: VendorInvoiceItem
            builder.OwnsOne(lineItem => lineItem.Item)
                .Property(item => item.PartNumber)
                .HasColumnName("PartNumber")
                .HasMaxLength(255)
                .IsRequired();

            //builder.OwnsOne(lineItem => lineItem.Item)
            //    .Property(item => item.Manufacturer)
            //    .HasColumnName("Manufacturer");

            builder.OwnsOne(lineItem => lineItem.Item)
               .Property(item => item.Description)
               .HasColumnName("Description")
               .HasMaxLength(255)
               .IsRequired();

            //builder.OwnsOne(lineItem => lineItem.Item)
            //    .Property(item => item.SaleCode)
            //    .HasColumnName("SaleCode");


        }
    }
}
