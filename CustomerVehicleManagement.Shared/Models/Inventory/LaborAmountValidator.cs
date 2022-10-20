﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class LaborAmountValidator : AbstractValidator<LaborAmountToWrite>
    {
        public LaborAmountValidator()
        {
            RuleFor(laborAmount => laborAmount)
                .NotEmpty()
                .MustBeValueObject(
                    laborAmount => LaborAmount.Create(
                        laborAmount.PayType,
                        laborAmount.Amount));
        }
    }
}
