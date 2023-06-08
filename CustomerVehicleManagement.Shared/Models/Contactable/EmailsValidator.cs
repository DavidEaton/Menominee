using CustomerVehicleManagement.Domain.Entities;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class EmailsValidator : AbstractValidator<IList<EmailToWrite>>
    {
        private const string onePrimarymessage = "Can have only one Primary email.";
        private const string notEmptyMessage = "Email must not be empty.";
        public EmailsValidator()
        {
            RuleFor(emails => emails)
                .NotNull()
                .Must(HaveOnlyOnePrimaryEmail)
                .WithMessage(onePrimarymessage)
                .ForEach(email =>
                {
                    email.NotEmpty().WithMessage(notEmptyMessage);
                    email.MustBeEntity(
                        emailtoWrite => 
                        Email.Create(
                            emailtoWrite.Address, 
                            emailtoWrite.IsPrimary));
                });
        }

        // This business rule should reside in the domain layer
        // rather than here in the application layer. However,
        // refactoring will have to wait for now...
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
