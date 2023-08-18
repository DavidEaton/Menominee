using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Configurations
{
    public class SaleCodeConfiguration : EntityConfiguration<SaleCode>
    {
        public override void Configure(EntityTypeBuilder<SaleCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("SaleCode", "dbo");

            builder.Property(saleCode => saleCode.Code)
                .IsRequired()
                .HasMaxLength(4);
            builder.Property(saleCode => saleCode.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(saleCode => saleCode.DesiredMargin)
                .HasDefaultValue(0);
            builder.HasCheckConstraint("Check_SaleCode_DesiredMargin", "[DesiredMargin] >= 0 AND [DesiredMargin] <= 100");
            builder.Property(saleCode => saleCode.LaborRate)
                .HasDefaultValue(0);
            //builder.Property(saleCode => saleCode.ShopSupplies)
            //    .IsRequired();
        }
    }
}
