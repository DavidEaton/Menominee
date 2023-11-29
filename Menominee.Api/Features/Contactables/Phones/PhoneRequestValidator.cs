using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;

namespace Menominee.Api.Features.Contactables.Phones
{
    public class PhoneRequestValidator : AbstractValidator<PhoneToWrite>
    {
        public PhoneRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(phone => phone)
                .NotEmpty()
                .WithMessage(Phone.EmptyMessage)
                .MustBeEntity(
                    phone => Phone.Create(
                    phone.Number,
                    phone.PhoneType,
                    phone.IsPrimary));
        }
    }
}
