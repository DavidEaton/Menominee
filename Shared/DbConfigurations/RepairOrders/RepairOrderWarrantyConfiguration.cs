using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenomineePlayWASM.Shared.DbConfigurations.RepairOrders
{
    public class RepairOrderWarrantyConfiguration : EntityConfiguration<RepairOrderWarranty>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderWarranty> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderWarranties", "dbo");
            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.Type)
               .HasDefaultValue(WarrantyType.NewWarranty)
               .IsRequired();
        }
    }
}
