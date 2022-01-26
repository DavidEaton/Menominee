using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderServiceConfiguration : EntityConfiguration<RepairOrderService>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderService> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderServices", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
