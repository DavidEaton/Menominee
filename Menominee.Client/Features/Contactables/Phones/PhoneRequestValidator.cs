using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using System.Text.RegularExpressions;

namespace Menominee.Client.Features.Contactables.Phones
{
    public class PhoneRequestValidator : AbstractValidator<PhoneToWrite>
    {
        public PhoneRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(phone => phone.Number)
                .NotEmpty()
                .WithMessage(Phone.EmptyMessage)
                .Must(BeAValidPhoneNumber)
                .WithMessage(Phone.InvalidMessage);
        }

        private static readonly Func<string, bool> BeAValidPhoneNumber = number =>
        {
            return !string.IsNullOrEmpty(number) && Regex.IsMatch(number, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        };

    }
}
