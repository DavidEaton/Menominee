using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderItemTaxConfiguration : EntityConfiguration<RepairOrderItemTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItemTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItemTax", "dbo");
        }
    }
}
