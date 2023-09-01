using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.DriversLicenses;

namespace Menominee.Shared.Models.Persons
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
                        person.Notes,
                        person.Birthday));
        }
    }
}
