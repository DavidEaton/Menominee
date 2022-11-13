using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Payables
{
    public class VendorConfiguration : EntityConfiguration<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            base.Configure(builder);
            builder.ToTable("Vendor", "dbo");

            builder.Property(vendor => vendor.VendorCode)
                .HasMaxLength(10)
                .IsRequired();
            builder.Property(vendor => vendor.Name)
                .HasMaxLength(255)
                .IsRequired();

            // Value Object: VendorInvoiceItem
            // Configure Value Object to contain Entity reference
            builder.OwnsOne(
                vendor => vendor.DefaultPaymentMethod, builder =>
                {
                    builder.WithOwner();
                    builder.Navigation(paymentMethod => paymentMethod.PaymentMethod)
                        .UsePropertyAccessMode(PropertyAccessMode.Property);
                });
            builder.OwnsOne(vendor => vendor.DefaultPaymentMethod)
                .Property(paymentMethod => paymentMethod.PaymentMethod)
                .HasColumnName("DefaultPaymentMethod");
            builder.OwnsOne(vendor => vendor.DefaultPaymentMethod)
                .Property(paymentMethod => paymentMethod.AutoCompleteDocuments)
                .HasColumnName("AutoCompleteDocuments");
        }
    }
}
