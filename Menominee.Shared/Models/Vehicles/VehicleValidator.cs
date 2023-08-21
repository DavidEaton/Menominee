using FluentValidation;
using Menominee.Domain.Entities;

namespace Menominee.Shared.Models.Vehicles;

public class VehicleValidator : AbstractValidator<VehicleToWrite>
{
    public VehicleValidator()
    {
        RuleFor(vehicle => vehicle)
            .MustBeEntity(vehicle =>
                Vehicle.Create(vehicle.VIN, vehicle.Year, vehicle.Make, vehicle.Model));
    }
}
