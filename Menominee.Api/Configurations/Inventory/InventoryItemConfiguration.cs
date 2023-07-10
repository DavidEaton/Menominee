using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class InventoryItemConfiguration : EntityConfiguration<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItem", "dbo");

            builder.Property(item => item.ItemNumber)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(item => item.Description)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(item => item.ItemType)
                .IsRequired();
        }
    }
}
