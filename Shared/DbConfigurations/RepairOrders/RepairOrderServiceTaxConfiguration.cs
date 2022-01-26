using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderServiceTaxConfiguration : EntityConfiguration<RepairOrderServiceTax>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderServiceTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderServiceTaxes", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
