using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PhonesToAddValidator : AbstractValidator<IList<PhoneToAdd>>
    {
        private const string message = "Can have only one Primary phone.";
        public PhonesToAddValidator()
        {
            RuleFor(phones => phones).Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(HaveOnlyOnePrimaryPhone)
                .WithMessage(message)
                .ForEach(phone =>
                {
                    phone.NotEmpty();
                    phone.MustBeEntity(x => Phone.Create(x.Number, x.PhoneType, x.IsPrimary));
                });
        }

        private bool HaveOnlyOnePrimaryPhone(IList<PhoneToAdd> phones)
        {
            int primaryCount = 0;

            foreach (var phone in phones)
            {
                if (phone.IsPrimary)
                    primaryCount += 1;
            }

            if (primaryCount > 1)
            {
                return false;
            }

            return true;
        }
    }
}
