using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemInspectionConfiguration : EntityConfiguration<InventoryItemInspection>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemInspection> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemInspection", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
