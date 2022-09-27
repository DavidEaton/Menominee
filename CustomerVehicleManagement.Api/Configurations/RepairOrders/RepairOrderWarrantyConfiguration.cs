using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderWarrantyConfiguration : EntityConfiguration<RepairOrderWarranty>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderWarranty> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderWarranty", "dbo");

            builder.Property(item => item.Type)
               .HasDefaultValue(WarrantyType.NewWarranty)
               .IsRequired();
        }
    }
}
