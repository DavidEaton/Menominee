using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryTireConfiguration : EntityConfiguration<InventoryTire>
    {
        public override void Configure(EntityTypeBuilder<InventoryTire> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryPart", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
