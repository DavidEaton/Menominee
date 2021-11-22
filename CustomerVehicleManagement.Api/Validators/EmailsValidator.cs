using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailsValidator : AbstractValidator<IList<EmailToWrite>>
    {
        private const string message = "Can have only one Primary email.";
        public EmailsValidator()
        {
            RuleFor(emails => emails).Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(HaveOnlyOnePrimaryEmail)
                //.ListHasNoMoreThanOnePrimary()
                .WithMessage(message)
                .ForEach(email =>
                {
                    email.NotEmpty();
                    email.MustBeEntity(x => Email.Create(x.Address, x.IsPrimary));
                });
        }

        private bool HaveOnlyOnePrimaryEmail(IList<EmailToWrite> emails)
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
