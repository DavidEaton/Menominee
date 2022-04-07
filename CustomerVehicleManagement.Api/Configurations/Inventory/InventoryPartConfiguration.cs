using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryPartConfiguration : EntityConfiguration<InventoryPart>
    {
        public override void Configure(EntityTypeBuilder<InventoryPart> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryPart", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
