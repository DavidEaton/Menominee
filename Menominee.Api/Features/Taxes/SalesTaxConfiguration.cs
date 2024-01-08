using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Taxes
{
    public class SalesTaxConfiguration : EntityConfiguration<SalesTax>
    {
        public override void Configure(EntityTypeBuilder<SalesTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("SalesTax", "dbo");

            builder.Property(salesTax => salesTax.Description)
                .IsRequired();
            builder.Property(salesTax => salesTax.TaxType)
                .HasDefaultValue(SalesTaxType.Normal);
            builder.Property(salesTax => salesTax.IsAppliedByDefault)
                .HasDefaultValue(true);
            builder.Property(salesTax => salesTax.IsTaxable)
                .HasDefaultValue(false);
            builder.Property(salesTax => salesTax.TaxIdNumber)
                .HasMaxLength(255);
            builder.Property(salesTax => salesTax.PartTaxRate)
                .HasDefaultValue(0);
            builder.Property(salesTax => salesTax.LaborTaxRate)
                .HasDefaultValue(0);

            //builder.HasMany(salesTax => salesTax.ExciseFees)
            //    .WithOne()
            //    .IsRequired()
            //    .HasForeignKey(exciseFee => exciseFee.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Metadata
            //    .FindNavigation("ExciseFees")
            //    .SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
