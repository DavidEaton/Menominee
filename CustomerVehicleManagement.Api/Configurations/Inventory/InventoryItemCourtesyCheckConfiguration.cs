using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemCourtesyCheckConfiguration : EntityConfiguration<InventoryItemCourtesyCheck>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemCourtesyCheck> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemCourtesyCheck", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
