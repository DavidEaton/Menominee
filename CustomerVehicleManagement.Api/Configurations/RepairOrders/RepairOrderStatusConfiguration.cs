using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
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
