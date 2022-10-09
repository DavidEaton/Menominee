using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemTireConfiguration : EntityConfiguration<InventoryItemTire>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemTire> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemTire", "dbo");

            builder.Property(packageItem => packageItem.Type)
                .HasMaxLength(2);
            builder.Property(packageItem => packageItem.Width)
                .IsRequired();
            builder.Property(packageItem => packageItem.AspectRatio)
                .IsRequired();
            builder.Property(packageItem => packageItem.Diameter)
                .IsRequired();

            // :InstallablePart
            builder.Property(part => part.LineCode)
                .HasMaxLength(255);

            builder.Property(part => part.SubLineCode)
                .HasMaxLength(255);

            builder.Property(part => part.Fractional)
                .IsRequired();

            // Value Object: InventoryItemPackageDetails
            builder.OwnsOne(part => part.TechAmount)
               .Property(techAmount => techAmount.PayType)
               .HasColumnName("TechPayType")
               .IsRequired();
            builder.OwnsOne(part => part.TechAmount)
               .Property(techAmount => techAmount.Amount)
               .HasColumnName("TechPayAmount")
               .IsRequired();
            builder.OwnsOne(part => part.TechAmount)
               .Property(techAmount => techAmount.SkillLevel)
               .HasColumnName("TechSkillLevel")
               .IsRequired();
        }
    }
}
