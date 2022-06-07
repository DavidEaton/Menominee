using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class ProductCodeConfiguration : EntityConfiguration<ProductCode>
    {
        public override void Configure(EntityTypeBuilder<ProductCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProductCode", "dbo");

            builder.Ignore(pc => pc.TrackingState);

            //builder.Property(pc => pc.Manufacturer)
            //    .IsRequired();
            builder.Property(pc => pc.Code)
                .IsRequired();
            builder.Property(pc => pc.Name)
                .IsRequired();

            builder.HasOne(pc => pc.Manufacturer)
                   .WithMany()
                   .HasForeignKey(pc => pc.ManufacturerId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(pc => pc.SaleCode)
                   .WithMany()
                   .HasForeignKey(pc => pc.SaleCodeId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }
}
