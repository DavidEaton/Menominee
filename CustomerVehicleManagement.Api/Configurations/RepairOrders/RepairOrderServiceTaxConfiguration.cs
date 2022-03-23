using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderServiceTaxConfiguration : EntityConfiguration<RepairOrderServiceTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderServiceTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderServiceTax", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
