using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemWarrantyConfiguration : EntityConfiguration<InventoryItemWarranty>
    {
        public class InventoryItemDonationConfiguration : EntityConfiguration<InventoryItemWarranty>
        {
            public override void Configure(EntityTypeBuilder<InventoryItemWarranty> builder)
            {
                base.Configure(builder);
                builder.ToTable("InventoryItemWarranty", "dbo");

                builder.Ignore(item => item.TrackingState);
            }
        }
    }
}
