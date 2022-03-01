using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using Menominee.Common.ValueObjects;
using System;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class PersonValidator : AbstractValidator<PersonToWrite>
    {
        public PersonValidator()
        {
            RuleFor(person => person.Name)
                                    .NotEmpty()
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

            // TODO: FIX THIS RULE; FAILS WHEN MODEL IS VALID
            //RuleFor(person => person.Gender)
            //                        .NotEmpty()
            //                        .IsInEnum()
            //                        .WithMessage("Please select a valid Gender");

            RuleFor(person => person.Birthday)
                                    .NotEmpty()
                                    .Must(BeValidAge)
                                    .WithMessage("Please enter a valid Birthday")
                                    .When(person => person.Birthday != null);

            RuleFor(person => person.DriversLicense.Issued)
                                    .Must(PreceedToday)
                                    .WithMessage("Issued date must not occur in the future")
                                    .When(person => person.DriversLicense != null);

            RuleFor(person => person.DriversLicense.Expiry)
                                    .GreaterThan(DateTime.Today)
                                    .WithMessage("Expiry date must not occur in the past")
                                    .GreaterThan(person => person.DriversLicense.Issued)
                                    .WithMessage("Expiry date must not occur before Issued date")

                                    .DependentRules(() =>
                                    {
                                        RuleFor(person => person.DriversLicense).NotEmpty()
                                            .MustBeValueObject(driversLicense => DriversLicense.Create(driversLicense.Number,
                                                                                                        driversLicense.State,
                                                                                                        DateTimeRange.Create(driversLicense.Issued,
                                                                                                                             driversLicense.Expiry).Value));

                                    }).When(person => person.DriversLicense != null); ;

            RuleFor(person => person.Emails)
                                    .NotNull()
                                    .SetValidator(new EmailsValidator());

            RuleFor(person => person.Phones)
                                    .NotNull()
                                    .SetValidator(new PhonesValidator());
        }

        private bool PreceedToday(DateTime arg)
        {
            return arg < DateTime.Today;
        }

        // This business rule should reside in the domain layer
        // rather than here in the application layer. However,
        // refactoring will have to wait for now...
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
