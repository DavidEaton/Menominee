using FluentValidation;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Persons;

namespace Menominee.Client.Features.Contactables.Persons
{
    public class PersonNameRequestValidator : AbstractValidator<PersonNameToWrite>
    {
        public PersonNameRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(name => name.FirstName)
                .NotEmpty()
                .WithMessage(PersonName.RequiredMessage)
                .Length(PersonName.MinimumLength, PersonName.MaximumLength)
                .WithMessage(PersonName.InvalidLengthMessage);

            RuleFor(name => name.LastName)
                .NotEmpty()
                .WithMessage(PersonName.RequiredMessage)
                .Length(PersonName.MinimumLength, PersonName.MaximumLength)
                .WithMessage(PersonName.InvalidLengthMessage);

            RuleFor(name => name.MiddleName)
                .Length(PersonName.MinimumLength, PersonName.MaximumLength)
                .WithMessage(PersonName.InvalidLengthMessage)
                .When(name => !string.IsNullOrWhiteSpace(name.MiddleName));
        }
    }
}
