using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class MaintenanceItemConfiguration : EntityConfiguration<MaintenanceItem>
    {
        public override void Configure(EntityTypeBuilder<MaintenanceItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("MaintenanceItem", "dbo");

            builder.Property(maintenanceItem => maintenanceItem.DisplayOrder)
                .IsRequired();
        }
    }
}
