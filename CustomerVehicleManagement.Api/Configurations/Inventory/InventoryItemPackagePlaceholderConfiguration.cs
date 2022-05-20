using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemPackagePlaceholderConfiguration : EntityConfiguration<InventoryItemPackagePlaceholder>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemPackagePlaceholder> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemPackagePlaceholder", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
