using FluentValidation;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Addresses;

namespace Menominee.Api.Features.Contactables.Addresses
{
    public class AddressRequestValidator : AbstractValidator<AddressToWrite>
    {
        public AddressRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(address => address)
                .NotEmpty()
                .MustBeValueObject(
                    address => Address.Create(
                        address.AddressLine1,
                        address.City,
                        address.State,
                        address.PostalCode,
                        address.AddressLine2));
        }
    }
}
