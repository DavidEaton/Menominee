using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.ProductCodes
{
    public class ProductCodeConfiguration : EntityConfiguration<ProductCode>
    {
        public override void Configure(EntityTypeBuilder<ProductCode> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProductCode", "dbo");

            builder.Property(productCode => productCode.Code)
                .IsRequired()
                .HasMaxLength(8);
            builder.HasCheckConstraint("Check_ProductCode_Code_Length", "[Code] IS NULL OR (LEN([Code]) >= 1 AND LEN([Code]) <= 8)");
            builder.Property(productCode => productCode.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
