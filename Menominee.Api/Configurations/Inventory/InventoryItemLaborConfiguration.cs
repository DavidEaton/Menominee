using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.Inventory
{
    public class InventoryItemLaborConfiguration : EntityConfiguration<InventoryItemLabor>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemLabor> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemLabor", "dbo");

            // Value Object: LaborAmount
            builder.OwnsOne(labor => labor.LaborAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("LaborPayType")
               .IsRequired();
            builder.OwnsOne(labor => labor.LaborAmount)
               .Property(amount => amount.Amount)
               .HasColumnName("LaborPayAmount")
               .IsRequired();

            // Value Object: TechAmount
            builder.OwnsOne(labor => labor.TechAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("TechPayType")
               .IsRequired();
            builder.OwnsOne(labor => labor.TechAmount)
               .Property(amount => amount.PayType)
               .HasColumnName("TechPayType")
               .IsRequired();
            builder.OwnsOne(labor => labor.TechAmount)
               .Property(amount => amount.SkillLevel)
               .HasColumnName("TechSkillLevel");
        }
    }
}
