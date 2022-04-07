using CustomerVehicleManagement.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations.Inventory
{
    public class ManufacturerConfiguration : EntityConfiguration<Manufacturer>
    {
        public override void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Manufacturer", "dbo");

            builder.Ignore(mfr => mfr.TrackingState);

            builder.Property(mfr => mfr.Code)
                .IsRequired();
            builder.Property(mfr => mfr.Name)
                .IsRequired();
        }
    }
}
