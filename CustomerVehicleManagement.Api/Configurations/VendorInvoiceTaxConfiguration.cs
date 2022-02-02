﻿using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class VendorInvoiceTaxConfiguration : EntityConfiguration<VendorInvoiceTax>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoiceTax> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoiceTax", "dbo");

            builder.Ignore(tax => tax.TrackingState);
            builder.Ignore(tax => tax.TaxName);

            builder.Property(tax => tax.InvoiceId)
                   .IsRequired();
        }
    }
}