using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class InventoryLaborConfiguration : EntityConfiguration<InventoryLabor>
    {
        public override void Configure(EntityTypeBuilder<InventoryLabor> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryLabor", "dbo");

            builder.Ignore(item => item.TrackingState);
        }
    }
}
