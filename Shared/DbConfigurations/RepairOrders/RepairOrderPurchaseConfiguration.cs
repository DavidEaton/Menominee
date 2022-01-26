using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderPurchaseConfiguration : EntityConfiguration<RepairOrderPurchase>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderPurchase> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderPurchases", "dbo");
            builder.Ignore(item => item.TrackingState);

        }
    }
}
