using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Shared.Models.Persons;

namespace Menominee.Api.Features.Contactables.Persons.PersonNames
{
    public class PersonNameRequestValidator : AbstractValidator<PersonNameToWrite>
    {
        public PersonNameRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(personName => personName)
            .MustBeValueObject(
                name => PersonName.Create(
                    name.LastName,
                    name.FirstName,
                    name.MiddleName));
        }
    }
}
