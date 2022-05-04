using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Api.Configurations
{
    public class ManufacturerConfiguration : EntityConfiguration<Manufacturer>
    {
        public override void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Manufacturer", "dbo");

            builder.Ignore(manufacturer => manufacturer.TrackingState);

            builder.Property(manufacturer => manufacturer.Code)
                .IsRequired();
            builder.Property(manufacturer => manufacturer.Name)
                .IsRequired();
        }
    }
}
