using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class VendorInvoicePaymentMethodConfiguration : EntityConfiguration<VendorInvoicePaymentMethod>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoicePaymentMethod> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoicePaymentMethod", "dbo");

            builder.Ignore(payment => payment.TrackingState);

            builder.Property(payment => payment.Name)
                   .HasMaxLength(255)
                   .IsRequired();
        }

    }
}
