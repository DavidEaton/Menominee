using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using System.Text.RegularExpressions;

namespace Menominee.Client.Features.Contactables.Emails
{
    public class EmailRequestValidator : AbstractValidator<EmailToWrite>
    {
        public EmailRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(email => email.Address)
                .NotEmpty()
                .WithMessage(Email.EmptyMessage)
                .Must(BeAValidEmail)
                .WithMessage(Email.InvalidMessage);
        }

        private bool BeAValidEmail(string emailAddress)
        {
            return !string.IsNullOrEmpty(emailAddress) && Regex.IsMatch(emailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

    }
}
