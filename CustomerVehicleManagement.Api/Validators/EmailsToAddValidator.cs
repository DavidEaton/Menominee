using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailsToAddValidator : AbstractValidator<IList<EmailToAdd>>
    {
        private const string message = "Can have only one Primary email.";
        public EmailsToAddValidator()
        {
            RuleFor(emails => emails)
                .NotNull()
                .Must(HaveOnlyOnePrimaryEmail)
                .WithMessage(message)
                .ForEach(email =>
                {
                    email.NotEmpty();
                    email.SetValidator(new EmailToAddValidator());
                });
        }

        private bool HaveOnlyOnePrimaryEmail(IList<EmailToAdd> emails)
        {
            int primaryCount = 0;

            foreach (var email in emails)
            {
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
