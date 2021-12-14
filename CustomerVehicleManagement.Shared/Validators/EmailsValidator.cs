using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class EmailsValidator : AbstractValidator<IList<EmailToWrite>>
    {
        private const string onePrimarymessage = "Can have only one Primary email.";
        private const string notEmptyMessage = "Email must not be empty.";
        public EmailsValidator()
        {
            RuleFor(emails => emails)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(HaveOnlyOnePrimaryEmail)
                .WithMessage(onePrimarymessage)
                .ForEach(email =>
                {
                    email.Cascade(CascadeMode.Stop);
                    email.NotEmpty().WithMessage(notEmptyMessage);
                    email.MustBeEntity(x => Email.Create(x.Address, x.IsPrimary));
                });
        }

        private bool HaveOnlyOnePrimaryEmail(IList<EmailToWrite> emails)
        {
            int primaryCount = 0;

            foreach (var email in emails)
            {
                if (email is null)
                    continue;

                if (email.IsPrimary)
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
