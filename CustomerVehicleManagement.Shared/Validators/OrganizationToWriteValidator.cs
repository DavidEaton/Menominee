using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class OrganizationToWriteValidator : AbstractValidator<OrganizationToWrite>
    {
        public OrganizationToWriteValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .Length(2, 255);

            RuleFor(_ => _.Note)
                .Length(0, 10000)
                .When(_ => _.Note != null);

            RuleFor(_ => _.Address)
                .SetValidator(new AddressToWriteValidator())
                .When(_ => _.Address != null);
        }
    }
}
