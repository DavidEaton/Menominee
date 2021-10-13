using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class AddressValidator : AbstractValidator<AddressToAdd>
    {
        public AddressValidator()
        {
            RuleFor(address => address.AddressLine).NotEmpty().Length(2, 255);
            RuleFor(address => address.City).NotEmpty().Length(2, 255);
            RuleFor(address => address.State).NotEmpty().IsInEnum().WithMessage("Must select a valid State");
            RuleFor(address => address.PostalCode).NotEmpty().Length(5, 10);
        }
    }
}
