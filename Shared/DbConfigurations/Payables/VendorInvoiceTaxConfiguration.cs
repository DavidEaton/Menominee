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
    public class VendorInvoiceTaxConfiguration : EntityConfiguration<VendorInvoiceTax>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceTax> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoiceTaxes", "dbo");

            builder.Ignore(tax => tax.TrackingState);

            builder.Property(tax => tax.InvoiceId)
                   .IsRequired();
        }
    }
}
