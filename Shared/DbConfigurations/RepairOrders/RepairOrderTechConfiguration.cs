using MenomineePlayWASM.Shared.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderTechConfiguration : EntityConfiguration<RepairOrderTech>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderTech> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderTechs", "dbo");
            builder.Ignore(item => item.TrackingState);
        }
    }
}
