using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.RepairOrders
{
    public class RepairOrderSerialNumberConfiguration : EntityConfiguration<RepairOrderSerialNumber>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderSerialNumber> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderSerialNumber", "dbo");

            builder.Property(item => item.SerialNumber)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
