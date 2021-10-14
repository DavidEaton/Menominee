using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PhoneToAddValidator : AbstractValidator<PhoneToAdd>
    {
        public PhoneToAddValidator()
        {
            RuleFor(phone => phone.Number)
                .NotEmpty()
                // https://www.rfc-editor.org/rfc/rfc5321#section-4.5.3
                .Matches(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")
                .WithMessage("Please enter a valid phone number");
        }
    }
}
