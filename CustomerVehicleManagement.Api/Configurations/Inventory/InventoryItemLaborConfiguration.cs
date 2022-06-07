using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemLaborConfiguration : EntityConfiguration<InventoryItemLabor>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemLabor> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemLabor", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
