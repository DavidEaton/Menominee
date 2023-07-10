using Menominee.Domain.Entities;
using FluentValidation;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Contactable
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
                        phoneToWrite =>
                        Phone.Create(
                            phoneToWrite.Number,
                            phoneToWrite.PhoneType,
                            phoneToWrite.IsPrimary));
                });
        }
    }
}
