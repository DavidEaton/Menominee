using Menominee.Api.Configurations;
using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Vehicles;

public class VehicleConfiguration : EntityConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);
        builder.ToTable("Vehicle", "dbo");

        builder.Property(vehicle => vehicle.VIN)
            .HasMaxLength(Vehicle.VinLength);

        builder.Property(vehicle => vehicle.Make)
            .HasMaxLength(Vehicle.MaximumLength);

        builder.Property(vehicle => vehicle.Model)
            .HasMaxLength(Vehicle.MaximumLength);
    }
}
