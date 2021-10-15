using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CustomerVehicleManagement.Api.Validators
{
    public class AddressToAddValidator : AbstractValidator<AddressToAdd>
    {
        public AddressToAddValidator()
        {
            RuleFor(address => address.AddressLine).NotEmpty().Length(2, 255);
            RuleFor(address => address.City).NotEmpty().Length(2, 255);
            RuleFor(address => address.State).NotEmpty().IsInEnum().WithMessage("Please select a valid State");
            RuleFor(address => address.PostalCode).NotEmpty().Length(5, 10).Must(BeAValidPostalcode).WithMessage("Must enter a valid Postal Code");
        }

        private bool BeAValidPostalcode(string postalcode)
        {
            if (postalcode == null || !Regex.Match(postalcode, @"^\d{5}(?:[-\s]\d{4})?$").Success)
                return false;

            return true;
        }
    }
}
