using CustomerVehicleManagement.Domain.Entities;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class PhonesValidator : AbstractValidator<IList<PhoneToWrite>>
    {
        private const string notEmptyMessage = "Phone must not be empty.";
        public PhonesValidator()
        {
            RuleFor(phones => phones)
                .NotNull()
                .ForEach(phone =>
                {
                    phone.NotEmpty()
                        .WithMessage(notEmptyMessage);
                    phone.MustBeEntity(
                        x => Phone.Create(
                            x.Number,
                            x.PhoneType,
                            x.IsPrimary));
                });
        }
    }
}
