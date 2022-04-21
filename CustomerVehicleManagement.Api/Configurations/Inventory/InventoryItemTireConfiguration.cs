using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryItemTireConfiguration : EntityConfiguration<InventoryItemTire>
    {
        public override void Configure(EntityTypeBuilder<InventoryItemTire> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItemTire", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
