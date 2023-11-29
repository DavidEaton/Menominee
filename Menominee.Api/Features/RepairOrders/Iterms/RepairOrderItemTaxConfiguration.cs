using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.RepairOrders.Iterms
{
    public class RepairOrderItemTaxConfiguration : EntityConfiguration<RepairOrderItemTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItemTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItemTax", "dbo");

            // Value Object: LaborTax
            builder.OwnsOne(lineItem => lineItem.LaborTax)
               .Property(laborTax => laborTax.Amount)
               .HasColumnName("LaborTaxAmount");
            builder.OwnsOne(lineItem => lineItem.LaborTax)
               .Property(laborAmount => laborAmount.Rate)
               .HasColumnName("LaborTaxRate");

            // Value Object: PartTax
            builder.OwnsOne(lineItem => lineItem.PartTax)
               .Property(partTax => partTax.Amount)
               .HasColumnName("PartTaxAmount");
            builder.OwnsOne(lineItem => lineItem.PartTax)
               .Property(partTax => partTax.Rate)
               .HasColumnName("PartTaxRate");
        }
    }
}
