using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Inventory
{
    public class InventoryItemPackageConfiguration : EntityConfiguration<InventoryItemPackage>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemPackage> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemPackage", "dbo");

            builder.Property(itemPackage => itemPackage.IsDiscountable)
                .IsRequired();
            builder.Property(itemPackage => itemPackage.Script)
                .HasMaxLength(InventoryItemPackage.ScriptMaximumLength);
        }
    }
}
