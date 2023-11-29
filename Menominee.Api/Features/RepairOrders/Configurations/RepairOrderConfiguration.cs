using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.RepairOrders.Configurations
{
    public class RepairOrderConfiguration : EntityConfiguration<RepairOrder>
    {
        public override void Configure(EntityTypeBuilder<RepairOrder> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrder", "dbo");

        }
    }
}
