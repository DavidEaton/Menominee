using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class VendorInvoiceConfiguration : EntityConfiguration<VendorInvoice>
    {
        public override void Configure(EntityTypeBuilder<VendorInvoice> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("VendorInvoice", "dbo");

            builder.Ignore(invoice => invoice.TrackingState);

            builder.Property(invoice => invoice.Date);

            builder.Property(invoice => invoice.Status)
                .HasDefaultValue(VendorInvoiceStatus.Open);
        }
    }
}
