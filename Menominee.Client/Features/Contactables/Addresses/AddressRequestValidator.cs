using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Shared.Models.Addresses;
using System.Text.RegularExpressions;

namespace Menominee.Client.Features.Contactables.Addresses
{
    public class AddressRequestValidator : AbstractValidator<AddressToWrite>
    {
        private static readonly string usPostalCodeRegEx = @"^[0-9]{5}(?:-[0-9]{4})?$";
        public AddressRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(address => address.AddressLine1)
                .NotEmpty()
                .WithMessage(Address.AddressRequiredMessage)
                .Length(Address.AddressMinimumLength, Address.AddressMaximumLength)
                .WithMessage(Address.AddressLengthMessage);

            RuleFor(address => address.AddressLine2)
                .Length(Address.AddressMinimumLength, Address.AddressMaximumLength)
                .WithMessage(Address.AddressLengthMessage)
                .When(address => !string.IsNullOrWhiteSpace(address.AddressLine2));

            RuleFor(address => address.City)
                .NotEmpty()
                .WithMessage(Address.CityRequiredMessage)
                .Length(Address.CityMinimumLength, Address.CityMaximumLength)
                .WithMessage(Address.CityLengthMessage);

            RuleFor(address => address.State)
                .IsInEnum()
                .WithMessage(Address.StateInvalidMessage);

            RuleFor(address => address.PostalCode)
                .NotEmpty()
                .WithMessage(Address.PostalCodeRequiredMessage)
                .Must(BeAValidPostalCode)
                .WithMessage(Address.PostalCodeInvalidMessage);
        }

        private bool BeAValidPostalCode(string postalCode)
        {
            return Regex.IsMatch(postalCode, usPostalCodeRegEx);
        }
    }
}
