using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.DbConfigurations.Payables
{
    public class VendorInvoicePaymentConfiguration : EntityConfiguration<VendorInvoicePayment>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoicePayment> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoicePayments", "dbo");

            builder.Ignore(payment => payment.TrackingState);

            builder.Property(payment => payment.InvoiceId)
                   .IsRequired();

            builder.Property(payment => payment.PaymentMethod)
                   .IsRequired();
        }
    }
}
