using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class VendorInvoicePaymentConfiguration : EntityConfiguration<VendorInvoicePayment>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoicePayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoicePayment", "dbo");

            builder.Ignore(payment => payment.TrackingState);
            builder.Ignore(payment => payment.PaymentMethodName);

            builder.Property(payment => payment.InvoiceId)
                   .IsRequired();

            builder.Property(payment => payment.PaymentMethod)
                   .IsRequired();
        }
    }
}
