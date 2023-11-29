using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using System.ComponentModel.DataAnnotations;

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

        private bool BeAValidPhoneNumber(string number)
        {
            var phoneAttribute = new PhoneAttribute();
            return phoneAttribute.IsValid(number);
        }
    }
}
