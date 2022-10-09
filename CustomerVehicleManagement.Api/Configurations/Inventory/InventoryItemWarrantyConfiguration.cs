using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemWarrantyConfiguration : EntityConfiguration<InventoryItemWarranty>
    {
        public class InventoryItemDonationConfiguration : EntityConfiguration<InventoryItemWarranty>
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
}
