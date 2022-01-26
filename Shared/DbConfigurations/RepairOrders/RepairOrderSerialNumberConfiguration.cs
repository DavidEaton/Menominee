using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderSerialNumberConfiguration : EntityConfiguration<RepairOrderSerialNumber>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderSerialNumber> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderSerialNumbers", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
