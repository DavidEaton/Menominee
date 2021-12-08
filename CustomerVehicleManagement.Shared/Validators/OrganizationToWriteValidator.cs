using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class OrganizationToWriteValidator : AbstractValidator<OrganizationToWrite>
    {
        public OrganizationToWriteValidator()
        {
            RuleFor(organization => organization.Name)
                .NotEmpty()
                .WithMessage("Organization name is required.");

            RuleFor(organization => organization.Note)
                .Length(0, 10000)
                .When(organization => organization.Note != null);

            //RuleFor(organization => organization.Address
        }
    }
}
