using FluentValidation;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;

namespace Menominee.Api.Features.Contactables.Emails
{
    public class EmailRequestValidator : AbstractValidator<EmailToWrite>
    {
        public EmailRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(email => email)
                .NotEmpty()
                .WithMessage(Email.EmptyMessage)
                .MustBeEntity(email =>
                    Email.Create(
                        email.Address,
                        email.IsPrimary));
        }
    }
}
