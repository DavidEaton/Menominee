using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderTaxConfiguration : EntityConfiguration<RepairOrderTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderTax", "dbo");
        }
    }
}
