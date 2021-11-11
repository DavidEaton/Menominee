using FluentValidation;
using Menominee.Common.ValueObjects;
using System.Text.RegularExpressions;

namespace CustomerVehicleManagement.Api.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address)
                .MustBeValueObject(address => Address.Create(address.AddressLine, address.City, address.State, address.PostalCode));

            RuleFor(address => address.AddressLine)
                                      .NotEmpty()
                                      .Length(2, 255);

            RuleFor(address => address.City)
                                      .NotEmpty()
                                      .Length(2, 255);

            RuleFor(address => address.State)
                                      .NotEmpty()
                                      .IsInEnum()
                                      .WithMessage("Please select a valid State");

            RuleFor(address => address.PostalCode)
                                      .NotEmpty()
                                      .Length(5, 10)
                                      .Must(IsValidPostalcode)
                                      .WithMessage("Please enter a valid Postal Code");
        }

        private bool IsValidPostalcode(string postalcode)
        {
            if (postalcode == null || !Regex.Match(postalcode, @"^\d{5}(?:[-\s]\d{4})?$").Success)
                return false;

            return true;
        }
    }
}
