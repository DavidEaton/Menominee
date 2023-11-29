using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.RepairOrders.Configurations
{
    public class RepairOrderPurchaseConfiguration : EntityConfiguration<RepairOrderPurchase>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderPurchase> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderPurchase", "dbo");

        }
    }
}
