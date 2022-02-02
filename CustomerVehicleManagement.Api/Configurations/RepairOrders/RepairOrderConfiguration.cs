using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderConfiguration : EntityConfiguration<RepairOrder>
    {
        public override void Configure(EntityTypeBuilder<RepairOrder> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrders", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
