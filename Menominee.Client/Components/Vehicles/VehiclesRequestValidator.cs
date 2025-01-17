﻿using FluentValidation;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Client.Components.Vehicles
{
    public class VehiclesRequestValidator : AbstractValidator<List<VehicleToWrite>>
    {
        public VehiclesRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vehicles => vehicles)
                .NotNull()
                .WithMessage("Vehicles list cannot be null");

            RuleFor(vehicles => vehicles)
                .ForEach(vehicle =>
                {
                    vehicle.SetValidator(new VehicleRequestValidator());
                });
        }
    }
}
