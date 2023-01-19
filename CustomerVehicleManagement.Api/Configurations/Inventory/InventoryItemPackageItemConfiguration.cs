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

            builder.HasOne(packageItem => packageItem.Item)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.Property(packageItem => packageItem.DisplayOrder)
                .IsRequired();

            // Value Object: InventoryItemPackageDetails
            builder.OwnsOne(packageItem => packageItem.Details)
               .Property(details => details.Quantity)
               .HasColumnName("Quantity")
               .IsRequired();
            builder.OwnsOne(packageItem => packageItem.Details)
               .Property(details => details.ExciseFeeIsAdditional)
               .HasColumnName("ExciseFeeIsAdditional")
               .IsRequired();
            builder.OwnsOne(packageItem => packageItem.Details)
               .Property(details => details.LaborAmountIsAdditional)
               .HasColumnName("LaborAmountIsAdditional")
               .IsRequired();
            builder.OwnsOne(packageItem => packageItem.Details)
               .Property(details => details.PartAmountIsAdditional)
               .HasColumnName("PartAmountIsAdditional")
               .IsRequired();
        }
    }
}
