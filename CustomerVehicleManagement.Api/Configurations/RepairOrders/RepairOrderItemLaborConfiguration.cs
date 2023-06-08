using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderItemLaborConfiguration : EntityConfiguration<RepairOrderItemLabor>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItemLabor> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItemLabor", "dbo");

            // Value Object: LaborAmount
            builder.OwnsOne(itemLabor => itemLabor.LaborAmount)
               .Property(labor => labor.Amount)
               .HasColumnName("LaborAmount");

            // Value Object: TechAmount
            builder.OwnsOne(itemLabor => itemLabor.TechAmount)
               .Property(tech => tech.Amount)
               .HasColumnName("TechAmount");
        }
    }
}
