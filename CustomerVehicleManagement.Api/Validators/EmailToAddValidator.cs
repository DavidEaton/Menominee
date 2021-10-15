using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailToAddValidator : AbstractValidator<EmailToAdd>
    {
        public EmailToAddValidator()
        {
            RuleFor(email => email.Address)
                .NotEmpty()
            // https://www.rfc-editor.org/rfc/rfc5321#section-4.5.3
                .Length(1, 254)
                .EmailAddress()
                .WithMessage("Please enter a valid email address.")
                .When(email => email.Address != null);
        }
    }
}
