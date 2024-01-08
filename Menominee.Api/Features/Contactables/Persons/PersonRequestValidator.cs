using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Addresses;
using Menominee.Api.Features.Contactables.Emails;
using Menominee.Api.Features.Contactables.Persons.DriverseLicenses;
using Menominee.Api.Features.Contactables.Persons.PersonNames;
using Menominee.Api.Features.Contactables.Phones;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Persons;
using System;

namespace Menominee.Api.Features.Contactables.Persons
{
    public class PersonRequestValidator : AbstractValidator<PersonToWrite>
    {
        private readonly ApplicationDbContext context;
        public PersonRequestValidator(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(person => person.Name)
                .SetValidator(new PersonNameRequestValidator());

            RuleFor(person => person.Address)
                .SetValidator(new AddressRequestValidator())
                .When(person => person.Address is not null && person.Address.IsNotEmpty);

            RuleFor(person => person.DriversLicense)
                .SetValidator(new DriversLicenseRequestValidator())
                .When(person => person.DriversLicense is not null && person.DriversLicense.IsNotEmpty);

            RuleFor(person => person.Emails)
                .SetValidator(new EmailsRequestValidator(context));

            RuleFor(person => person.Phones)
                .SetValidator(new PhonesRequestValidator());

            RuleFor(person => person)
                .MustBeEntity(
                person =>
                {
                    if (person.Name is null)
                    {
                        return Result.Failure<Person>(Contactable.RequiredMessage);
                    }

                    var nameResult = PersonName.Create(person.Name.LastName, person.Name.FirstName, person.Name.MiddleName);
                    return nameResult.IsFailure
                        ? Result.Failure<Person>(nameResult.Error)
                        : Person.Create(
                        nameResult.Value,
                        person.Notes,
                        person.Birthday);
                });
        }
    }
}
