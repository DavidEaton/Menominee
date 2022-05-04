using CustomerVehicleManagement.Domain.Entities.Taxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Taxes
{
    public class SalesTaxConfiguration : EntityConfiguration<SalesTax>
    {
        public override void Configure(EntityTypeBuilder<SalesTax> builder)
        {
            base.Configure(builder);
            builder.ToTable("SalesTax", "dbo");

            builder.Ignore(st => st.TrackingState);

            builder.Property(st => st.Description)
                .IsRequired();
        }
    }
}
