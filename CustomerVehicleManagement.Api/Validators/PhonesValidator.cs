using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PhonesValidator : AbstractValidator<IList<PhoneToAdd>>
    {
        private const string message = "Can have only one Primary phone.";
        public PhonesValidator()
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
            int primaryPhoneCount = 0;

            foreach (var phone in phones)
            {
                if (phone.IsPrimary)
                    primaryPhoneCount += 1;
            }

            if (primaryPhoneCount > 1)
            {
                return false;
            }

            return true;
        }
    }
}
