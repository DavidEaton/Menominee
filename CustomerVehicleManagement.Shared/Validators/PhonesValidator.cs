using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class PhonesValidator : AbstractValidator<IList<PhoneToWrite>>
    {
        private const string onePrimarymessage = "Can have only one Primary phone.";
        private const string notEmptyMessage = "Phone must not be empty.";
        public PhonesValidator()
        {
            RuleFor(phones => phones)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(HaveOnlyOnePrimaryPhone)
                .WithMessage(onePrimarymessage)
                .ForEach(phone =>
                {
                    phone.Cascade(CascadeMode.Stop);
                    phone.NotEmpty().WithMessage(notEmptyMessage);
                    phone.MustBeEntity(x => Phone.Create(x.Number, x.PhoneType, x.IsPrimary));
                });
        }

        private bool HaveOnlyOnePrimaryPhone(IList<PhoneToWrite> phones)
        {
            int primaryCount = 0;

            foreach (var phone in phones)
            {
                if (phone is null)
                    continue;

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
