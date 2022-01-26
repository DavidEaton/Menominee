using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderItemTaxConfiguration : EntityConfiguration<RepairOrderItemTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItemTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItemTaxes", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
