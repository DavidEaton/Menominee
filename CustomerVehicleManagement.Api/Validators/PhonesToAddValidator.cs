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
            RuleFor(phones => phones)
                .Must(HaveOnlyOnePrimaryPhone)
                .WithMessage(message)
                .ForEach(phone =>
                {
                    phone.NotEmpty();
                    phone.SetValidator(new PhoneToAddValidator());
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
