using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Inventory
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
