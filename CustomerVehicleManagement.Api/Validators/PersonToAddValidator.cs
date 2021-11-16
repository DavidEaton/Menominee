using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PersonToAddValidator : AbstractValidator<PersonToAdd>
    {
        public PersonToAddValidator()
        {
            //RuleFor(person => person.Name)
            //                        .MustBeValueObject(PersonName.Create());

            RuleFor(person => person.Gender)
                                    .NotEmpty()
                                    .IsInEnum()
                                    .WithMessage("Please select a valid Gender");

            RuleFor(person => person.Birthday)
                .Must(BeValidAge)
                .WithMessage("Please enter a valid Birthday")
                .When(person => person.Birthday != null);

            //RuleFor(person => person.DriversLicense)
            //                        .MustBeValueObject(DriversLicense.Create())
            //                        .When(driversLicense => driversLicense != null);;

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
