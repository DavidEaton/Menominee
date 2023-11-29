using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Components.Vehicles
{
    public class VehicleRequestValidator : AbstractValidator<VehicleToWrite>
    {
        public VehicleRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vehicle => vehicle.VIN)
                .Length(Vehicle.VinLength)
                .WithMessage(Vehicle.InvalidVinMessage)
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.VIN));

            RuleFor(vehicle => vehicle.Year)
                .InclusiveBetween(Vehicle.YearMinimum, DateTime.Today.Year + 1)
                .WithMessage(Vehicle.InvalidYearMessage)
                .When(vehicle => vehicle.Year.HasValue);

            RuleFor(vehicle => vehicle.Make)
                .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength)
                .When(vehicle => !vehicle.NonTraditionalVehicle)
                .WithMessage(Vehicle.InvalidLengthMessage);

            RuleFor(vehicle => vehicle.Model)
                .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength)
                .When(vehicle => !vehicle.NonTraditionalVehicle)
                .WithMessage(Vehicle.InvalidLengthMessage);

            RuleFor(vehicle => vehicle)
                .Must(vehicle => !vehicle.NonTraditionalVehicle || !string.IsNullOrWhiteSpace(vehicle.Make) || !string.IsNullOrWhiteSpace(vehicle.Model))
                .WithMessage(Vehicle.NonTraditionalVehicleInvalidMakeModelMessage)
                .When(vehicle => vehicle.NonTraditionalVehicle);

            RuleFor(vehicle => vehicle.Plate)
                .MaximumLength(Vehicle.MaximumPlateLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumPlateLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.Plate));

            RuleFor(vehicle => vehicle.PlateStateProvince)
                .IsInEnum()
                .WithMessage(Vehicle.InvalidPlateStateProvinceMessage)
                .When(vehicle => vehicle.PlateStateProvince.HasValue);

            RuleFor(vehicle => vehicle.UnitNumber)
                .MaximumLength(Vehicle.MaximumUnitNumberLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumUnitNumberLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.UnitNumber));

            RuleFor(vehicle => vehicle.Color)
                .MaximumLength(Vehicle.MaximumColorLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumColorLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.Color));
        }
    }
}
