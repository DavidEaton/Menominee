using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Contactable;
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
                .NotNull()
                .Must(HaveOnlyOnePrimaryPhone)
                .WithMessage(onePrimarymessage)
                .ForEach(phone =>
                {
                    phone.NotEmpty().WithMessage(notEmptyMessage);
                    phone.MustBeEntity(x => Phone.Create(x.Number, x.PhoneType, x.IsPrimary));
                });
        }

        // This business rule should reside in the domain layer
        // rather than here in the application layer. However,
        // refactoring will have to wait for now...
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
