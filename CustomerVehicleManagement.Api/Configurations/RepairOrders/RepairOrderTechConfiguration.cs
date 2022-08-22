using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderTechConfiguration : EntityConfiguration<RepairOrderTech>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderTech> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderTech", "dbo");
        }
    }
}
