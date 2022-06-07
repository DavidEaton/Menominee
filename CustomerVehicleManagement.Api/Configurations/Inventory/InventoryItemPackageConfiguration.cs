using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemPackageConfiguration : EntityConfiguration<InventoryItemPackage>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemPackage> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemPackage", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
