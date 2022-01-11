using MenomineePlayWASM.Shared.Entities.Inventory;
using MenomineePlayWASM.Shared.Entities.Inventory.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.DbConfigurations.Inventory
{
    public class InventoryItemConfiguration : EntityConfiguration<InventoryItem>
    {
        public override void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("InventoryItems", "dbo");

            builder.Ignore(item => item.TrackingState);

            builder.Property(item => item.PartNumber)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.Description)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(item => item.PartType)
                   .HasDefaultValue(InventoryItemType.Part)
                   .IsRequired();
        }
    }
}
