using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;

namespace Menominee.Api.Features.Contactables.Phones
{
    public class PhonesRequestValidator : ItemsWithPrimaryValidator<PhoneToWrite, PhoneRequestValidator>
    {
        public PhonesRequestValidator() : base()
        {
            RuleFor(phones => phones)
                .ForEach(phone =>
                {
                    phone
                        .NotEmpty()
                        .WithMessage(Phone.EmptyMessage);
                });
        }
    }
}
