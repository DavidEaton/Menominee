﻿using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using Menominee.Common.ValueObjects;
using System;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PersonValidator : AbstractValidator<PersonToWrite>
    {
        public PersonValidator()
        {
            RuleFor(person => person.Name)
                                    .MustBeValueObject(person => PersonName.Create(person.LastName,
                                                                                   person.FirstName,
                                                                                   person.MiddleName));

            RuleFor(person => person.Address)
                                    .NotEmpty()
                                    .MustBeValueObject(address => Address.Create(address.AddressLine,
                                                                                 address.City,
                                                                                 address.State,
                                                                                 address.PostalCode))
                                    .When(organization => organization.Address != null);

            RuleFor(person => person.Gender)
                                    .NotEmpty()
                                    .IsInEnum()
                                    .WithMessage("Please select a valid Gender");

            RuleFor(person => person.Birthday)
                .Must(BeValidAge)
                .WithMessage("Please enter a valid Birthday")
                .When(person => person.Birthday != null);

            RuleFor(person => person.DriversLicense)
                                    .MustBeValueObject(driversLicense => DriversLicense.Create(driversLicense.Number,
                                                                                               driversLicense.State,
                                                                                               DateTimeRange.Create(driversLicense.Issued,
                                                                                                                    driversLicense.Expiry).Value))
                                    .When(driversLicense => driversLicense != null);

            RuleFor(person => person.Emails)
                .NotNull()
                .SetValidator(new EmailsValidator());

            RuleFor(person => person.Phones)
                .NotNull()
                .SetValidator(new PhonesValidator());

        }
        protected bool BeValidAge(DateTime? date)
        {
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Value.Year;

            if (dobYear <= currentYear && dobYear > (currentYear - 120))
            {
                return true;
            }

            return false;
        }
    }
}