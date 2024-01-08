using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Components.Vehicles
{
    public class VehicleRequestValidator : AbstractValidator<VehicleToWrite>
    {
        public VehicleRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vehicle => vehicle)
                .Must(vehicle => vehicle is not null && !vehicle.IsEmpty())
                .WithMessage("Vehicle is invalid or empty");
            // Making each rule into a method is useful for debugging, but they are not used by any application components.
            var resultVIN = ValidateVIN();
            var resultYear = ValidateYear();
            var resultMake = ValidateMake();
            var resultModel = ValidateModel();
            var resultNonTraditionalVehicle = ValidateNonTraditionalVehicle();
            var resultePlate = ValidatePlate();
            var resultPlateStateProvince = ValidatePlateStateProvince();
            var resultUnitNumber = ValidateUnitNumber();
            var resultColor = ValidateColor();
        }

        // This method is useful for debugging, but is not used by any application components.
        public List<string> ValidateAndGetErrors(VehicleToWrite vehicle)
        {
            var result = Validate(vehicle);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidateVIN()
        {
            return RuleFor(vehicle => vehicle.VIN)
                .Length(Vehicle.VinLength)
                .WithMessage(Vehicle.InvalidVinMessage)
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.VIN));
        }

        private IRuleBuilderOptions<VehicleToWrite, int?> ValidateYear()
        {
            return RuleFor(vehicle => vehicle.Year)
                .InclusiveBetween(Vehicle.YearMinimum, DateTime.Today.Year + 1)
                .WithMessage(Vehicle.InvalidYearMessage)
                .When(vehicle => vehicle.Year.HasValue);
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidateMake()
        {
            return RuleFor(vehicle => vehicle.Make)
                .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength)
                .WithMessage(Vehicle.InvalidLengthMessage)
                .When(vehicle => !vehicle.NonTraditionalVehicle);
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidateModel()
        {
            return RuleFor(vehicle => vehicle.Model)
                .Length(Vehicle.MinimumMakeModelLength, Vehicle.MaximumMakeModelLength)
                .WithMessage(Vehicle.InvalidLengthMessage)
                .When(vehicle => !vehicle.NonTraditionalVehicle);
        }

        private IRuleBuilderOptions<VehicleToWrite, VehicleToWrite> ValidateNonTraditionalVehicle()
        {
            return RuleFor(vehicle => vehicle)
                .Must(vehicle => !string.IsNullOrWhiteSpace(vehicle.Make) || !string.IsNullOrWhiteSpace(vehicle.Model))
                .WithMessage(Vehicle.NonTraditionalVehicleInvalidMakeModelMessage)
                .When(vehicle => vehicle.NonTraditionalVehicle);
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidatePlate()
        {
            return RuleFor(vehicle => vehicle.Plate)
                .MaximumLength(Vehicle.MaximumPlateLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumPlateLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.Plate));
        }

        private IRuleBuilderOptions<VehicleToWrite, State?> ValidatePlateStateProvince()
        {
            return RuleFor(vehicle => vehicle.PlateStateProvince)
                .IsInEnum()
                .WithMessage(Vehicle.InvalidPlateStateProvinceMessage)
                .When(vehicle => vehicle.PlateStateProvince.HasValue);
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidateUnitNumber()
        {
            return RuleFor(vehicle => vehicle.UnitNumber)
                .MaximumLength(Vehicle.MaximumUnitNumberLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumUnitNumberLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.UnitNumber));
        }

        private IRuleBuilderOptions<VehicleToWrite, string> ValidateColor()
        {
            return RuleFor(vehicle => vehicle.Color)
                .MaximumLength(Vehicle.MaximumColorLength)
                .WithMessage(Vehicle.InvalidMaximumLengthMessage(Vehicle.MaximumColorLength))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.Color));
        }
    }
}
