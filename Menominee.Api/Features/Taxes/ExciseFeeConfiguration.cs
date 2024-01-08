using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Taxes
{
    public class ExciseFeeConfiguration : EntityConfiguration<ExciseFee>
    {
        public override void Configure(EntityTypeBuilder<ExciseFee> builder)
        {
            base.Configure(builder);
            builder.ToTable("ExciseFee", "dbo");

            builder.Property(fee => fee.Description)
                .IsRequired()
                .HasMaxLength(ExciseFee.DescriptionMaximumLength);
            builder.Property(fee => fee.FeeType)
                .IsRequired()
                .HasDefaultValue(ExciseFeeType.Flat);
            builder.Property(fee => fee.Amount)
                .HasDefaultValue(0);
        }
    }
}
