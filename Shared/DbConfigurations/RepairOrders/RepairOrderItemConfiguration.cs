using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using MenomineePlayWASM.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderItemConfiguration : EntityConfiguration<RepairOrderItem>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItems", "dbo");
            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.SaleType)
               .HasDefaultValue(SaleType.Regular)
               .IsRequired();

            builder.Property(item => item.PartType)
               .HasDefaultValue(PartType.Part)
               .IsRequired();
        }
    }
}
