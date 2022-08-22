using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class SaleCodeConfiguration : EntityConfiguration<SaleCode>
    {
        public override void Configure(EntityTypeBuilder<SaleCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("SaleCode", "dbo");

            builder.Property(saleCode => saleCode.Code)
                .IsRequired();
            builder.Property(saleCode => saleCode.Name)
                .IsRequired();
            builder.Property(saleCode => saleCode.DesiredMargin)
                .HasDefaultValue(0);
            builder.Property(saleCode => saleCode.LaborRate)
                .HasDefaultValue(0);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.IncludeLabor)
               .HasColumnName("IncludeLabor")
               .HasDefaultValue(false);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.IncludeParts)
               .HasColumnName("IncludeParts")
               .HasDefaultValue(false);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.MaximumCharge)
               .HasColumnName("MaximumCharge")
                .HasDefaultValue(0);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.MinimumCharge)
               .HasColumnName("MinimumCharge")
                .HasDefaultValue(0);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.MinimumJobAmount)
               .HasColumnName("MinimumJobAmount")
                .HasDefaultValue(0);
            builder.OwnsOne(saleCode => saleCode.ShopSupplies)
               .Property(shopSupplies => shopSupplies.Percentage)
               .HasColumnName("Percentage")
                .HasDefaultValue(0);
        }
    }
}
