using Menominee.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class VendorInvoicePaymentConfiguration : EntityConfiguration<VendorInvoicePayment>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoicePayment> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoicePayment", "dbo");

        }
    }
}
