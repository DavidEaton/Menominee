using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderServiceTaxConfiguration : EntityConfiguration<RepairOrderServiceTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderServiceTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderServiceTax", "dbo");

            // Value Object: LaborTax
            builder.OwnsOne(serviceTax => serviceTax.LaborTax)
               .Property(laborTax => laborTax.Amount)
               .HasColumnName("LaborTaxAmount");
            builder.OwnsOne(serviceTax => serviceTax.LaborTax)
               .Property(laborAmount => laborAmount.Rate)
               .HasColumnName("LaborTaxRate");

            // Value Object: PartTax
            builder.OwnsOne(serviceTax => serviceTax.PartTax)
               .Property(partTax => partTax.Amount)
               .HasColumnName("PartTaxAmount");
            builder.OwnsOne(serviceTax => serviceTax.PartTax)
               .Property(partTax => partTax.Rate)
               .HasColumnName("PartTaxRate");
        }
    }
}
