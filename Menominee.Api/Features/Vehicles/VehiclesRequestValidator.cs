using FluentValidation;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;

namespace Menominee.Api.Features.Vehicles
{
    public class VehiclesRequestValidator : AbstractValidator<List<VehicleToWrite>>
    {
        public VehiclesRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vehicles => vehicles)
                .ForEach(vehicle =>
                {
                    vehicle.SetValidator(new VehicleRequestValidator());
                });
        }
    }
}
