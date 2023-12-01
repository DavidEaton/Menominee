using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;
using System.Linq;

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

        // This method is useful for debugging, but is not used by any application components.
        public List<string> ValidateAndGetErrors(VehicleToWrite vehicle)
        {
            var result = Validate(vehicle);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}
