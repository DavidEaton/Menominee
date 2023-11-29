using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;

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
                .EmailAddress()
                .WithMessage(Email.InvalidMessage);
        }
    }
}
