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
    public class VendorInvoiceItemConfiguration : EntityConfiguration<VendorInvoiceItem>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceItem> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoiceItems", "dbo");

            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.PartNumber)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.MfrId)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(item => item.Description)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.Type)
                   .HasDefaultValue(VendorInvoiceItemType.Purchase)
                   .IsRequired();
        }
    }
}
