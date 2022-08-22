using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderServiceConfiguration : EntityConfiguration<RepairOrderService>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderService> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderService", "dbo");
        }
    }
}
