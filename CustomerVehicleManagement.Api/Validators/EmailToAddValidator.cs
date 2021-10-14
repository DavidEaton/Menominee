using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailToAddValidator : AbstractValidator<IList<EmailToAdd>>
    {
        private const string message = "Can have only one Primary email.";
        public EmailToAddValidator()
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
            int primaryEmailCount = 0;

            foreach (var email in emails)
            {
                if (email.IsPrimary)
                    primaryEmailCount += 1;
            }

            if (primaryEmailCount > 1)
            {
                return false;
            }

            return true;
        }

    }
}
