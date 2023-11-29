using Menominee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menominee.Api.Features.Vehicles;

public class VehicleConfiguration : EntityConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);
        builder.ToTable("Vehicle", "dbo");

        builder.Property(vehicle => vehicle.VIN)
            .HasMaxLength(Vehicle.VinLength);

        builder.Property(vehicle => vehicle.Make)
            .HasMaxLength(Vehicle.MaximumMakeModelLength);

        builder.Property(vehicle => vehicle.Model)
            .HasMaxLength(Vehicle.MaximumMakeModelLength);

        builder.Property(vehicle => vehicle.Plate)
            .HasMaxLength(Vehicle.MaximumPlateLength);

        builder.Property(vehicle => vehicle.UnitNumber)
            .HasMaxLength(Vehicle.MaximumUnitNumberLength);

        builder.Property(vehicle => vehicle.Color)
            .HasMaxLength(Vehicle.MaximumColorLength);

        builder.Property(vehicle => vehicle.Active)
            .HasDefaultValue(true);
    }
}
