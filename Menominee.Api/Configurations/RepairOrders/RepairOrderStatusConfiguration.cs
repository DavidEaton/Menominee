using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.RepairOrders
{
    public class RepairOrderStatusConfiguration : EntityConfiguration<RepairOrderStatus>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderStatus> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderStatus", "dbo");

            builder.Property(status => status.Description)
                .HasMaxLength(10_000);

        }
    }
}
