using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderTaxConfiguration : EntityConfiguration<RepairOrderTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderTax", "dbo");

            // Value Object: LaborTax
            builder.OwnsOne(orderTax => orderTax.LaborTax)
               .Property(laborTax => laborTax.Amount)
               .HasColumnName("LaborTaxAmount");
            builder.OwnsOne(orderTax => orderTax.LaborTax)
               .Property(laborTax => laborTax.Rate)
               .HasColumnName("LaborTaxRate");

            // Value Object: PartTax
            builder.OwnsOne(orderTax => orderTax.PartTax)
               .Property(partTax => partTax.Amount)
               .HasColumnName("PartTaxAmount");
            builder.OwnsOne(orderTax => orderTax.PartTax)
               .Property(partTax => partTax.Rate)
               .HasColumnName("PartTaxRate");
        }
    }
}
