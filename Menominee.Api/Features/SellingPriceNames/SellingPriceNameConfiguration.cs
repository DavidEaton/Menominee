using Menominee.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.SellingPriceNames;

public class SellingPriceNameConfiguration : EntityConfiguration<SellingPriceName>
{
    public override void Configure(EntityTypeBuilder<SellingPriceName> builder)
    {
        base.Configure(builder);

        builder.ToTable("SellingPriceName", "dbo");

        builder.Property(sellingPriceName => sellingPriceName.Name)
            .HasMaxLength(SellingPriceName.MaximumNameLength)
            .IsRequired();
    }
}
