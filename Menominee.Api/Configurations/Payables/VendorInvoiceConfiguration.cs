﻿using Menominee.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class VendorInvoiceConfiguration : EntityConfiguration<VendorInvoice>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoice> builder)
        {
            base.Configure(builder);
            builder.ToTable("VendorInvoice", "dbo");

            builder.Property(invoice => invoice.Status)
                .HasDefaultValue(VendorInvoiceStatus.Open);

            builder.Property(invoice => invoice.InvoiceNumber)
                .HasMaxLength(255);
        }
    }
}