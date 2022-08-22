using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderItemConfiguration : EntityConfiguration<RepairOrderItem>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItem", "dbo");

            builder.Property(item => item.SaleType)
               .HasDefaultValue(SaleType.Regular)
               .IsRequired();

            builder.Property(item => item.PartType)
               .HasDefaultValue(PartType.Part)
               .IsRequired();
        }
    }
}
