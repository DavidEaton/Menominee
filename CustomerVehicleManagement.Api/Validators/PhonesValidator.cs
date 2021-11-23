using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PhonesValidator : AbstractValidator<IList<PhoneToWrite>>
    {
        private const string message = "Can have only one Primary phone.";
        public PhonesValidator()
        {
            RuleFor(phones => phones).Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(HaveOnlyOnePrimaryPhone)
                //.ListHasNoMoreThanOnePrimary()
                .WithMessage(message)
                .ForEach(phone =>
                {
                    phone.NotEmpty();
                    phone.MustBeEntity(x => Phone.Create(x.Number, x.PhoneType, x.IsPrimary));
                });
        }

        private bool HaveOnlyOnePrimaryPhone(IList<PhoneToWrite> phones)
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
