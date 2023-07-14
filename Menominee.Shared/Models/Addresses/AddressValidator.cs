﻿using FluentValidation;
using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.Addresses
{
    public class AddressValidator : AbstractValidator<AddressToWrite>
    {
        public AddressValidator()
        {
            RuleFor(address => address)
                .NotEmpty()
                .MustBeValueObject(
                    address => Address.Create(
                        address.AddressLine,
                        address.City,
                        address.State,
                        address.PostalCode));
        }
    }
}