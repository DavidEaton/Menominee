using Menominee.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class VendorInvoicePaymentMethodConfiguration : EntityConfiguration<VendorInvoicePaymentMethod>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoicePaymentMethod> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoicePaymentMethod", "dbo");

            builder.Property(payment => payment.Name)
                   .HasMaxLength(255)
                   .IsRequired();
        }

    }
}
