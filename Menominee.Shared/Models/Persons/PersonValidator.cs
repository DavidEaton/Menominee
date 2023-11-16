using CSharpFunctionalExtensions;
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
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(person => person.Name)
                .MustBeValueObject(
                    name => PersonName.Create(
                        name.LastName,
                        name.FirstName,
                        name.MiddleName));

            RuleFor(person => person.Address)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new AddressValidator())
                .When(person => person.Address is not null);

            RuleFor(person => person.DriversLicense)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new DriversLicenseValidator())
                .When(person => person.DriversLicense is not null);

            RuleFor(person => person.Emails)
                .Cascade(CascadeMode.Continue)
                .NotNull()
                .SetValidator(new EmailsValidator());

            RuleFor(person => person.Phones)
                .Cascade(CascadeMode.Continue)
                .NotNull()
                .SetValidator(new PhonesValidator());

            RuleFor(person => person)
                .MustBeEntity((person) =>
                {
                    var nameResult = PersonName.Create(person.Name.LastName, person.Name.FirstName, person.Name.MiddleName);
                    if (nameResult.IsFailure)
                    {
                        return Result.Failure<Person>(nameResult.Error);
                    }

                    return Person.Create(
                            nameResult.Value,
                            person.Gender,
                            person.Notes,
                            person.Birthday);
                });
        }
    }
}
