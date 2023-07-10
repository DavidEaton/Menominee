using Menominee.Domain.Entities.RepairOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations.RepairOrders
{
    public class RepairOrderLineItemConfiguration : EntityConfiguration<RepairOrderLineItem>
    {
        public override void Configure(EntityTypeBuilder<RepairOrderLineItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("RepairOrderLineItem", "dbo");

            //builder.Property(item => item.Item)
            //    .IsRequired();
            builder.Property(item => item.SaleType)
                .IsRequired();

            // Value Object: LaborAmount
            builder.OwnsOne(lineItem => lineItem.LaborAmount)
               .Property(laborAmount => laborAmount.Amount)
               .HasColumnName("LaborAmount");
            builder.OwnsOne(person => person.LaborAmount)
               .Property(laborAmount => laborAmount.PayType)
               .HasColumnName("LaborPayType");

            // Value Object: DiscountAmount
            builder.OwnsOne(lineItem => lineItem.DiscountAmount)
               .Property(discountAmount => discountAmount.Type)
               .HasColumnName("DiscountType");
            builder.OwnsOne(person => person.DiscountAmount)
               .Property(discountAmount => discountAmount.Amount)
               .HasColumnName("DiscountAmount");

        }
    }
}
