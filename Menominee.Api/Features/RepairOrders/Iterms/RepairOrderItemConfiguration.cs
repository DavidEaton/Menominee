using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.RepairOrders.Iterms
{
    public class RepairOrderItemConfiguration : EntityConfiguration<RepairOrderItem>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderItem", "dbo");
            builder.Property(item => item.PartNumber)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(item => item.Description)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
