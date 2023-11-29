using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.RepairOrders.Iterms
{
    public class RepairOrderItemPartConfiguration : EntityConfiguration<RepairOrderItemPart>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItemPart> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItemPart", "dbo");

            // Value Object: TechAmount
            builder.OwnsOne(part => part.TechAmount, techAmount =>
            {
                techAmount.Property(p => p.Type).HasConversion<string>();
                techAmount.Property(p => p.Amount);
                techAmount.Property(p => p.SkillLevel).HasConversion<string>();
            });
        }
    }
}
