using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class InventoryItemWarrantyConfiguration : EntityConfiguration<InventoryItemWarranty>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemWarranty> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemWarranty", "dbo");

            // Value Object: LaborAmount
            builder.OwnsOne(warranty => warranty.WarrantyPeriod)
               .Property(period => period.PeriodType)
               .HasColumnName("PeriodType")
               .IsRequired();
            builder.OwnsOne(inspection => inspection.WarrantyPeriod)
               .Property(amount => amount.Duration)
               .HasColumnName("Duration")
               .IsRequired();
        }
    }
}
