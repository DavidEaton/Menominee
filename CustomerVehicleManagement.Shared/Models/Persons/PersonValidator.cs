using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.DriversLicenses;
using CustomerVehicleManagement.Shared.Validators;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Persons
{
    public class PersonValidator : AbstractValidator<PersonToWrite>
    {
        public PersonValidator()
        {
            RuleFor(person => person.Name)
                .MustBeValueObject(
                    name => PersonName.Create(
                        name.LastName,
                        name.FirstName,
                        name.MiddleName));

            RuleFor(person => person.Address)
                .SetValidator(new AddressValidator())
                .When(person => person.Address is not null);

            RuleFor(person => person.DriversLicense)
                .SetValidator(new DriversLicenseValidator())
                .When(person => person.DriversLicense is not null);

            RuleFor(person => person.Emails)
                .NotNull()
                .SetValidator(new EmailsValidator());

            RuleFor(person => person.Phones)
                .NotNull()
                .SetValidator(new PhonesValidator());

            RuleFor(person => person)
                .MustBeEntity(
                    person => Person.Create(
                        PersonName.Create(
                            person.Name.LastName,
                            person.Name.FirstName,
                            person.Name.MiddleName).Value,
                        person.Gender,
                        person.Birthday));
        }
    }
}
