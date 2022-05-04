using CustomerVehicleManagement.Domain.Entities.Taxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Taxes
{
    public class ExciseFeeConfiguration : EntityConfiguration<ExciseFee>
    {
        public override void Configure(EntityTypeBuilder<ExciseFee> builder)
        {
            base.Configure(builder);
            builder.ToTable("ExciseFee", "dbo");

            builder.Ignore(fee => fee.TrackingState);

            builder.Property(fee => fee.Description)
                .IsRequired();
        }
    }
}
