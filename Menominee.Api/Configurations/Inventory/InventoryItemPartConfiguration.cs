using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class InventoryItemPartConfiguration : EntityConfiguration<InventoryItemPart>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemPart> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemPart", "dbo");

            // :InstallablePart
            builder.Property(part => part.LineCode)
                .HasMaxLength(255);

            builder.Property(part => part.SubLineCode)
                .HasMaxLength(255);

            builder.Property(part => part.Fractional)
                .IsRequired();

            // Value Object: InventoryItemPackageDetails
            builder.OwnsOne(part => part.TechAmount)
               .Property(techAmount => techAmount.Type)
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
