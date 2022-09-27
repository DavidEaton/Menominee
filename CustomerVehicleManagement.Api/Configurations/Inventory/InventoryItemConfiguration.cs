using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemConfiguration : EntityConfiguration<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItem", "dbo");

            builder.Property(item => item.ItemNumber)
                .IsRequired();
            builder.Property(item => item.Description)
                .IsRequired();
        }
    }
}
