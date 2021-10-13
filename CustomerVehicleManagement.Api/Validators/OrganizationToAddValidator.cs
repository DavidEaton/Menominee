using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class OrganizationToAddValidator : AbstractValidator<OrganizationToAdd>
    {
        public OrganizationToAddValidator()
        {
            RuleFor(organization => organization.Name).NotEmpty().Length(2, 255);
            RuleFor(organization => organization.Address).SetValidator(new AddressValidator());

        }
    }
}
