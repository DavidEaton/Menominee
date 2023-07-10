using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class InventoryItemInspectionConfiguration : EntityConfiguration<InventoryItemInspection>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemInspection> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemInspection", "dbo");

            builder.Property(inspection => inspection.InspectionType)
                .IsRequired();

            // Value Object: LaborAmount
            builder.OwnsOne(inspection => inspection.LaborAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("LaborPayType")
               .IsRequired();
            builder.OwnsOne(inspection => inspection.LaborAmount)
               .Property(amount => amount.Amount)
               .HasColumnName("LaborPayAmount")
               .IsRequired();

            // Value Object: TechAmount
            builder.OwnsOne(inspection => inspection.TechAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("TechPayType")
               .IsRequired();
            builder.OwnsOne(inspection => inspection.TechAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("TechPayType")
               .IsRequired();
            builder.OwnsOne(inspection => inspection.TechAmount)
               .Property(amount => amount.SkillLevel)
               .HasColumnName("TechSkillLevel");

        }
    }
}
