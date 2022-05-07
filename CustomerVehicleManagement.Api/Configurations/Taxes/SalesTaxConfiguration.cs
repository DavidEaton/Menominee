using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Taxes
{
    public class SalesTaxConfiguration : EntityConfiguration<SalesTax>
    {
        public override void Configure(EntityTypeBuilder<SalesTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("SalesTax", "dbo");

            builder.Ignore(st => st.TrackingState);

            builder.Property(st => st.Description)
                .IsRequired();
            builder.Property(st => st.TaxType)
                .HasDefaultValue(SalesTaxType.Normal);
            builder.Property(st => st.IsAppliedByDefault)
                .HasDefaultValue(true);
            builder.Property(st => st.IsTaxable)
                .HasDefaultValue(false);
            builder.Property(st => st.TaxIdNumber)
                .HasMaxLength(255);
            builder.Property(st => st.PartTaxRate)
                .HasDefaultValue(0);
            builder.Property(st => st.LaborTaxRate)
                .HasDefaultValue(0);
        }
    }
}
