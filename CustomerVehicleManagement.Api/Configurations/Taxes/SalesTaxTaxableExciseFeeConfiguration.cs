using CustomerVehicleManagement.Domain.Entities.Taxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Taxes
{
    public class SalesTaxTaxableExciseFeeConfiguration : EntityConfiguration<SalesTaxTaxableExciseFee>
    {
        public override void Configure(EntityTypeBuilder<SalesTaxTaxableExciseFee> builder)
        {
            base.Configure(builder);
            builder.ToTable("SalesTaxTaxableExciseFee", "dbo");

            builder.Ignore(x => x.TrackingState);
        }
    }
}
