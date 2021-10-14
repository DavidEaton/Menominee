using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailsValidator : AbstractValidator<IList<EmailToAdd>>
    {
        private const string message = "Can have only one Primary email.";
        public EmailsValidator()
        {
            RuleFor(emails => emails)
                .Must(HaveOnlyOnePrimaryEmail)
                .WithMessage(message)
                .ForEach(email =>
                {
                    email.NotEmpty();
                    email.SetValidator(new EmailValidator());
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
