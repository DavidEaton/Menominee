using FluentValidation;
using Menominee.Domain.Entities;
using System;

namespace Menominee.Shared.Models.Vehicles;

public class VehicleValidator : AbstractValidator<VehicleToWrite>
{
    public VehicleValidator()
    {
        RuleFor(vehicle => vehicle.Year)
            .InclusiveBetween(Vehicle.YearMinimum, DateTime.Now.Year);

        RuleFor(vehicle => vehicle.Make)
            .NotNull()
            .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength);

        RuleFor(vehicle => vehicle.Model)
            .NotNull()
            .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength);

        RuleFor(vehicle => vehicle.Color)
            .MaximumLength(Vehicle.MaximumColorLength);

        RuleFor(vehicle => vehicle.UnitNumber)
            .MaximumLength(Vehicle.MaximumUnitNumberLength);

        RuleFor(vehicle => vehicle.Plate)
            .MaximumLength(Vehicle.MaximumPlateLength);

        RuleFor(vehicle => vehicle.PlateStateProvince)
            .IsInEnum();

        RuleFor(vehicle => vehicle.VIN)
            .Length(Vehicle.VinLength);

        RuleFor(vehicle => vehicle)
            .MustBeEntity(vehicle =>
                Vehicle.Create(vehicle.VIN, vehicle.Year, vehicle.Make, vehicle.Model, vehicle.Plate, vehicle.PlateStateProvince, vehicle.UnitNumber, vehicle.Color, vehicle.Active));
    }
}
