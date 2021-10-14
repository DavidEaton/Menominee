using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class EmailValidator : AbstractValidator<EmailToAdd>
    {
        public EmailValidator()
        {
            RuleFor(email => email.Address)
                .NotEmpty()
            // https://www.rfc-editor.org/rfc/rfc5321#section-4.5.3
                .Length(1, 254)
                .EmailAddress();
        }
    }
}
