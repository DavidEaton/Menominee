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

            builder.Property(placeholder => placeholder.ItemType)
                .IsRequired();

            builder.Property(placeholder => placeholder.Description)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(placeholder => placeholder.DisplayOrder)
                .IsRequired();

            // Value Object: InventoryItemPackageDetails
            builder.OwnsOne(placeholder => placeholder.Details)
               .Property(details => details.Quantity)
               .HasColumnName("Quantity")
               .IsRequired();
            builder.OwnsOne(placeholder => placeholder.Details)
               .Property(details => details.ExciseFeeIsAdditional)
               .HasColumnName("ExciseFeeIsAdditional")
               .IsRequired();
            builder.OwnsOne(placeholder => placeholder.Details)
               .Property(details => details.LaborAmountIsAdditional)
               .HasColumnName("LaborAmountIsAdditional")
               .IsRequired();
            builder.OwnsOne(placeholder => placeholder.Details)
               .Property(details => details.PartAmountIsAdditional)
               .HasColumnName("PartAmountIsAdditional")
               .IsRequired();
        }
    }
}
