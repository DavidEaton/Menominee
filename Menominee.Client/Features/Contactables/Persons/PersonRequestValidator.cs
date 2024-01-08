using FluentValidation;
using Menominee.Client.Features.Contactables.Addresses;
using Menominee.Client.Features.Contactables.Emails;
using Menominee.Client.Features.Contactables.Persons.DriversLicenses;
using Menominee.Client.Features.Contactables.Phones;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;

namespace Menominee.Client.Features.Contactables.Persons
{
    public class PersonRequestValidator : AbstractValidator<PersonToWrite>
    {
        public PersonRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(person => person.Name)
                .NotEmpty()
                .SetValidator(new PersonNameRequestValidator());

            RuleFor(person => person.Birthday)
                .Must(birthday => Person.IsValidAgeOn(birthday))
                .WithMessage(Contactable.InvalidValueMessage)
                .When(person => person.Birthday.HasValue);

            RuleFor(person => person.Address)
                .SetValidator(new AddressRequestValidator())
                .When(person => person.Address is not null && person.Address.IsNotEmpty);

            RuleFor(person => person.DriversLicense)
                .SetValidator(new DriversLicenseRequestValidator())
                .When(person => person.DriversLicense is not null && person.DriversLicense.IsNotEmpty);

            RuleFor(person => person.Emails)
                .SetValidator(new EmailsRequestValidator());

            RuleFor(person => person.Phones)
                .SetValidator(new PhonesRequestValidator());

            RuleFor(person => person.Notes)
                .Length(1, Contactable.NoteMaximumLength)
                .WithMessage(Contactable.NoteMaximumLengthMessage)
                .When(person => !string.IsNullOrWhiteSpace(person.Notes));
        }
    }
}