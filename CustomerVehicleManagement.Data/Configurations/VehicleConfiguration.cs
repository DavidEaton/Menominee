using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerVehicleManagement.Data.Configurations
{
    public class VehicleConfiguration : EntityConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            base.Configure(builder); // <--
            builder.ToTable("Vehicle", "dbo");
            builder.Ignore(vehicle => vehicle.TrackingState);
        }
    }
}
