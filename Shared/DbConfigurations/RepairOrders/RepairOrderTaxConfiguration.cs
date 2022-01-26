using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderTaxConfiguration : EntityConfiguration<RepairOrderTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderTaxes", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
