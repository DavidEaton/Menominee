using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Api.Features.Vehicles
{
    public class VehicleRequestValidator : AbstractValidator<VehicleToWrite>
    {
        public VehicleRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vehicle => vehicle)
                .NotEmpty()
                .MustBeEntity(vehicle =>
                    Vehicle.Create(
                        vehicle.VIN,
                        vehicle.Year,
                        vehicle.Make,
                        vehicle.Model,
                        vehicle.Plate,
                        vehicle.PlateStateProvince,
                        vehicle.UnitNumber,
                        vehicle.Color,
                        vehicle.Active,
                        vehicle.NonTraditionalVehicle));
        }
    }
}
