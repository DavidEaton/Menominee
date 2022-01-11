using MenomineePlayWASM.Shared.Entities.Payables.Enums;
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
    public class VendorInvoiceConfiguration : EntityConfiguration<VendorInvoice>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoice> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoices", "dbo");

            builder.Ignore(invoice => invoice.TrackingState);

            builder.Property(invoice => invoice.Date);
                //.HasDefaultValue(DateTime.Now);
            builder.Property(invoice => invoice.Status)
                .HasDefaultValue(VendorInvoiceStatus.Open);
        }
    }
}
