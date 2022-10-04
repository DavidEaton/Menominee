using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemPackageItemConfiguration : EntityConfiguration<InventoryItemPackageItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemPackageItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemPackageItem", "dbo");

            builder.HasOne(i => i.InventoryItem)
                   .WithMany()
                   .HasForeignKey(i => i.InventoryItemId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
