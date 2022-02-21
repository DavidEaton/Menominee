using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class InventoryItemConfiguration : EntityConfiguration<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("InventoryItem", "dbo");

            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.PartNumber)
                .IsRequired();
            builder.Property(item => item.Description)
                .IsRequired();
        }
    }
}
